<script>
export default {
	props: {
		title: {
			type: String,
			default: "Site Analytics",
		},
	},
	data() {
		return {
			labels: ["12am", "3am", "6am", "9am", "12pm", "3pm", "6pm", "9pm"],
			value: [
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
				Math.floor(Math.random() * 99),
			],
		};
	},
};
</script>

<template>
	<DashboardModule
		label="Global Analytics"
		icon="mdi-chart-bar-stacked"
	>
		<div class="mb-4">
			<div class="elevation-3">
				<v-sparkline
					:labels="labels"
					:value="value"
					color="blue"
					line-width="1"
					padding="16"
					auto-draw
				>
					<template #[`label`]="item">
						{{ item.value }}
					</template>
				</v-sparkline>
			</div>
		</div>
	</DashboardModule>
</template>
<style
	lang="scss"
	scoped
>
.custom-graph {
	border: solid 1px black;
}
</style>
