<template>
	<div class="container mt-12">
		<v-img
			src="/img/logo-black.svg"
			max-width="270px"
			alt="core 2.0"
		/>
		<h1>
			<v-icon
				large
				class="mr-2 mb-1"
				>mdi-lock-check</v-icon
			>Logging you out securely
		</h1>
		<p class="ml-11">Goodbye, {{ fullName }}.</p>
		<v-progress-circular
			class="ml-11"
			indeterminate
			size="100"
		/>
	</div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
export default {
	layout: "blank",
	computed: {
		...mapGetters({
			fullName: "user/fullName",
		}),
	},
	async mounted() {
		setTimeout(async () => {
			try {
				await this.$auth.logout();
				this.clearEverything();
			} catch (err) {
				throw new Error("Unauthorized");
			}
		}, 4000);
	},
	methods: {
		...mapMutations("site", ["clearEverything"]),
	},
};
</script>
<style
	lang="scss"
	scoped
>
.container {
	max-width: 1000px !important;
}
h1 {
	font-weight: 200;
	font-size: 48px;
}
</style>
