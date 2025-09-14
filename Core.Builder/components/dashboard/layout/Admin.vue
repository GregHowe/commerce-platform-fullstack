<template>
	<v-container fluid>
		<v-row>
			<v-col
				cols="12"
				lg="4"
			>
				<DashboardWidgetSiteTableTabs
					widget-title="My Sites"
					:site-list="siteList"
				/>
			</v-col>
			<v-col
				cols="12"
				lg="4"
			>
				<DashboardWidgetUserInfo />
			</v-col>
			<v-col
				cols="12"
				lg="4"
				><DashboardWidgetGlobalAnalytics
			/></v-col>
		</v-row>
	</v-container>
</template>

<script>
import { mapActions, mapGetters, mapState } from "vuex";
export default {
	props: {
		siteList: {
			type: Array,
			default: () => [],
		},
	},
};
</script>
