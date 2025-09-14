<template>
	<v-container fluid>
		<v-row>
			<v-col
				cols="12"
				lg="8"
			>
				<DashboardWidgetComplianceNotifications />
			</v-col>
		</v-row>
		<v-row>
			<v-col
				cols="12"
				lg="12"
			>
				<DashboardWidgetLatestContent />
			</v-col>
		</v-row>
		<v-row>
			<v-col
				cols="12"
				lg="5"
			>
				<DashboardWidgetSiteTable />
			</v-col>
			<v-col
				cols="12"
				lg="5"
			>
				<DashboardWidgetGlobalAnalytics />
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
import { mapActions } from "vuex";
export default {
	methods: {
		...mapActions("site", ["createSite", "removeSite"]),
	},
};
</script>
