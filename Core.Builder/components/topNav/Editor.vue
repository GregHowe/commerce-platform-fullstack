<template>
	<v-app-bar
		app
		elevation="3"
		clipped-left
		color="white"
		height="64"
	>
		<span class="text-h6 mr-4"
			><NuxtLink
				class="white--text"
				to="/"
				><v-img
					src="/img/logo-black.svg"
					max-width="120px"
					alt="core 2.0" /></NuxtLink
		></span>
		<v-btn
			text
			exact
			:to="{
				name: 'site-siteId',
				params: { siteId: selectedSiteId },
			}"
		>
			<v-icon
				class="mr-2"
				color="white"
				>mdi-file-document-multiple-outline</v-icon
			>
			{{ workingSite ? workingSite.title : "--" }}
		</v-btn>
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
