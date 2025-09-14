<template>
	<v-app-bar
		app
		elevation="3"
		clipped-left
		height="64"
	>
		<span class="text-h6 mr-4 ml-12"
			><NuxtLink to="/"
				><v-img
					src="/img/logo-black.svg"
					max-width="120px"
					alt="core 2.0" /></NuxtLink
		></span>
		<ForgeTooltip top>
			<template #trigger>
				<v-btn
					text
					exact
					:to="{
						name: 'site-siteId',
						params: { siteId: selectedSiteId },
					}"
				>
					<v-icon class="mr-2">mdi-web</v-icon
					>{{ workingSite ? workingSite.title : "--" }}
				</v-btn>
			</template>
			<template #default> Back to website dashboard </template>
		</ForgeTooltip>
		<v-spacer />

		<v-spacer />
		<TopNavAvatar />
	</v-app-bar>
</template>
<script>
import { mapState } from "vuex";
export default {
	computed: {
		...mapState("site", ["selectedSiteId", "workingSite"]),
	},
};
</script>
<style scoped>
.logo {
	border-right: solid 1px white;
}
</style>
