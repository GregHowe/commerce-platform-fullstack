<template>
	<div class="nylbg py-12 mx-auto">
		<v-card
			class="logincard mx-auto"
			max-width="374"
		>
			<v-card-text
				v-if="hasPermission('createsite')"
				class="text-center text--black"
			>
				<div class="my-4">
					<img
						class="mb-6"
						src="/img/nyl-logo.svg"
					/>
					<h3 class="mb-1">Welcome!</h3>
					<p>
						You're only a couple steps from your new website. Let's
						get started.
					</p>
					<v-btn
						id="loginbtn"
						class="mt-6"
						color="black"
						dark
					>
						Get started
					</v-btn>
					<h4 class="mt-8 text--lighten-1">POWERED BY CORE 2.0</h4>
				</div>
			</v-card-text>
			<v-card-text
				v-else
				class="text-center text--black"
			>
				<div class="my-4">
					<v-icon
						class="mb-8"
						color="red"
					>
						mdi-alert
					</v-icon>
					<h3 class="mb-1">Not authorized</h3>
					<p>
						You are not yet eligible to sign in. Please contact New
						York Life for more information.
					</p>
					<v-btn
						id="loginbtn"
						class="mt-6"
						color="black"
						dark
					>
						Contact Us
					</v-btn>
					<h4 class="mt-8 text--lighten-1">POWERED BY CORE 2.0</h4>
				</div>
			</v-card-text>
		</v-card>
	</div>
</template>

<script>
import { mapGetters } from "vuex";

export default {
	name: "DashboardEligibilityNotAuthorized",
	computed: {
		...mapGetters("user", ["hasPermission"]),
	},
};
</script>
