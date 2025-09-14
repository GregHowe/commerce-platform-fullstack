<template>
	<v-container>
		<v-row>
			<v-col cols="12">
				<h2>Lead forms test harness</h2>
			</v-col>
			<v-col
				cols="12"
				md="2"
			>
				<v-text-field
					v-model="brandId"
					dense
					outlined
					label="Brand ID"
				/>
			</v-col>
			<v-col
				cols="12"
				md="2"
			>
				<v-text-field
					v-model="siteId"
					dense
					outlined
					label="Site ID"
				/>
			</v-col>
			<v-col cols="12"> Site will load automatically </v-col>
		</v-row>
		<v-row>
			<iframe
				frameBorder="0"
				:src="iframeURL"
				height="800px"
				width="100%"
			/>
		</v-row>
	</v-container>
</template>
<script>
export default {
	data() {
		return {
			brandId: 3,
			siteId: 35,
		};
	},
	computed: {
		iframeURL() {
			return `https://f92core-${this.brandId}-${this.siteId}-staging.azureedge.net`;
		},
	},
};
</script>
