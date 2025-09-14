<template>
	<DashboardModule
		label="Core Sites"
		icon="mdi-web"
	>
		<v-tabs
			v-model="tab"
			slider-color="blue"
		>
			<v-tab>My Sites</v-tab>
			<v-tab>Pilot Sites</v-tab>
			<v-tab>All Sites</v-tab>
		</v-tabs>
		<v-tabs-items v-model="tab">
			<v-tab-item
				><DashboardWidgetSiteTable :site-list="sitesOwnedByMe"
			/></v-tab-item>
			<v-tab-item
				><DashboardWidgetSiteTable :site-list="sitesPilot"
			/></v-tab-item>
			<v-tab-item
				><DashboardWidgetSiteTable :site-list="siteList"
			/></v-tab-item>
		</v-tabs-items>
	</DashboardModule>
</template>
<script>
import { mapActions, mapGetters, mapState } from "vuex";
export default {
	data() {
		return {
			tab: "",
		};
	},
	computed: {
		...mapState("site", ["siteList"]),
		...mapGetters("user", { currentUserId: "emailAddress" }),
		sitesOwnedByMe() {
			return (
				this.siteList.filter(
					(item) => item.userId === this.currentUserId
				) || []
			);
		},
		sitesOwnedByOthers() {
			return (
				this.siteList.filter(
					(item) => item.userId !== this.currentUserId
				) || []
			);
		},
		sitesPilot() {
			return (
				this.siteList.filter(
					(item) =>
						item.id == "154" ||
						item.id == "192" ||
						item.id == "156" ||
						item.id == "157" ||
						item.id == "193" ||
						item.id == "194" ||
						item.id == "195" ||
						item.id == "196" ||
						item.id == "197" ||
						item.id == "198" ||
						item.id == "213"
				) || []
			);
		},
	},
	methods: {
		...mapActions("site", ["createSite", "removeSite"]),
	},
};
</script>
