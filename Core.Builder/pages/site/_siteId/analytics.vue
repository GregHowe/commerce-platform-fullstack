<template>
	<div class="container">
		<h1>Site Analytics</h1>
		<v-container>
			<v-row>
				<v-col
					cols="12"
					md="6"
				>
					<SamplesAnalyticsGraph />
				</v-col>
				<v-col
					cols="12"
					md="6"
				>
					<SamplesAnalyticsGraph title="Activity" />
				</v-col>
			</v-row>
		</v-container>
	</div>
</template>
<script>
export default {
	layout: "site",
};
</script>
