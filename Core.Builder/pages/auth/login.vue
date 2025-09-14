<template>
	<div class="rectbg py-12 mx-auto">
		<v-card class="logincard mx-auto">
			<v-card-text class="text-center">
				<div class="my-4">
					<v-img
						src="/img/logo-black.svg"
						width="155px"
						alt="core 2.0"
						class="mx-auto"
					/>
					<h2 class="welcome mt-4">WELCOME TO CORE 2.0</h2>
				</div>
				<FormLogin />
			</v-card-text>
		</v-card>
	</div>
</template>

<script>
import { mapState } from "vuex";

export default {
	layout: "login",
	auth: false,
	computed: {
		...mapState("site", ["siteList", "themeList"]),
	},
	mounted() {
		this.$auth.loggedIn ? this.$router.push("/") : "";
		if (
			!this.$auth.$state.redirect ||
			this.$auth.$state.redirect.includes("auth/logout")
		) {
			this.$auth.$storage.setUniversal("redirect", "/");
		}
	},
};
</script>

<style
	lang="scss"
	scoped
>
.welcome {
	font-size: 16px;
	color: black;
}
.logincard {
	max-width: 400px;
}
h1 {
	font-weight: 200;
	font-size: 48px;
}
</style>
