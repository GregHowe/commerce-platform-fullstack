<template>
	<v-container>
		<form
			align="center"
			@submit.prevent="login"
		>
			<editor-string
				id="login-username"
				v-model="input.username"
				label="Username"
				autocomplete="username"
				outlined
				full-width
				dense
			/>
			<editor-string
				id="login-password"
				v-model="input.password"
				type="password"
				label="Password"
				autocomplete="current-password"
				outlined
				full-width
				dense
			/>
			<div class="text-left forgotpassword">
				<a>Forgot Password</a>
			</div>
			<v-btn
				v-show="!isLoggingIn"
				id="loginbtn"
				class="mt-6 text-uppercase"
				type="submit"
				color="black"
				dark
				@click="login"
			>
				Login
			</v-btn>
			<v-progress-circular
				v-show="isLoggingIn"
				indeterminate
			/>
			<div class="py-4 error--text text-caption text-italic">
				{{ error }}
				{{ authError }}
			</div>
		</form>

		<v-row
			v-show="hasSso"
			class="mb-5"
			align="center"
		>
			<v-divider />
			<span class="mx-5">or</span>
			<v-divider />
		</v-row>
		<v-row
			v-for="sso in brandSettings.sso"
			:key="sso.id"
			class="my-5"
			align="center"
			justify="center"
		>
			<v-btn
				color="primary"
				large
				outlined
				class="mx-4 my-2 text-uppercase"
				:href="sso.url"
			>
				Continue with {{ sso.title }}
			</v-btn>
		</v-row>
	</v-container>
</template>
<script>
import { find as _find } from "lodash";
import { mapGetters, mapState } from "vuex";
export default {
	data() {
		return {
			isLoggingIn: false,
			input: {
				username: "",
				password: "",
			},
			ssoUrl: "",
			ssoTitle: "",
			error: "",
		};
	},
	computed: {
		...mapState("brand", {
			brandSettings: "settings",
		}),
		...mapState("interface", {
			authError: "authError",
		}),
		...mapGetters({
			isLoadingAnything: "interface/isLoadingAnything",
		}),
		hasSso() {
			return this.brandSettings?.sso?.length;
		},
	},
	async mounted() {
		this.ssoUrl = this.brandSettings?.sso.url;
		this.ssoTitle = this.brandSettings?.sso.title;
		this.input.username = this.$config.DEV_USERNAME;
		this.input.password = this.$config.DEV_PW;
	},
	methods: {
		async ssoLogin(issuer) {
			this.selectIssuer(issuer);
			const provider = _find(this.ssoProviders, { issuer });
			try {
				this.error = "";
				this.$store.commit("site/clearEverything");
				await this.$auth.requestWith("fusion", provider.url);
			} catch (err) {
				if (err.response) {
					this.error = `Login failed: ${err.response.status}: ${err.response.data}`;
				}
			}
		},
		async login() {
			this.$store.commit("interface/setAuthError", "");
			this.isLoggingIn = true;
			this.error = "";
			this.$store.commit("site/clearEverything");
			this.$auth
				.loginWith("local", {
					data: this.input,
				})
				.then(() => {
					if (!this.$auth.$state.redirect.includes("auth")) {
						this.$router.push(this.$auth.$state.redirect);
					} else {
						this.$router.push("/");
					}
				})
				.catch((err) => {
					if (err.response) {
						this.isLoggingIn = false;
						this.error = `Login failed: ${err.response.status}: ${err.response.data}`;
					}
				});
		},
	},
};
</script>
<style
	scoped
	lang="scss"
>
.forgotpassword {
	top: -14px;
	position: relative;
}
.forgotpassword a {
	color: black;
	text-decoration: underline;
	font-weight: bold;
}
</style>
