<template>
	<div>
		<SiteEnvironmentsLink
			v-for="env in Object.keys(workingSiteAzureURL)"
			:key="env"
			:env="env"
			:url="workingSiteAzureURL[env]"
		/>
		<v-icon color="blue darken-3">mdi-eye</v-icon>
		<ForgeChip
			small
			name="IN-BUILDER PREVIEW"
			:to="`/site/${$route.params.siteId}/page/${pageId}/preview`"
			target="_blank"
			color="blue darken-3"
			dark
			class="d-inline font-weight-bold"
		/>
	</div>
</template>
<script>
import { mapState, mapGetters } from "vuex";
export default {
	computed: {
		...mapState("site", ["workingSite", "workingPage"]),
		...mapGetters("site", ["workingSiteAzureURL"]),
		pageId() {
			if (this.workingSite?.pages && this.workingSite.pages.length) {
				return this.workingSite.pages[0]?.id;
			}
			return "";
		},
	},
};
</script>
