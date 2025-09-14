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
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
				Math.floor(Math.random() * 999),
			],
		};
	},
};
</script>

<template>
	<div class="mb-4">
		<h2 class="mb-2 accent--text">{{ title }}</h2>
		<div class="elevation-3">
			<v-sparkline
				:labels="labels"
				:value="value"
				color="secondary"
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
</template>
<style
	lang="scss"
	scoped
>
.custom-graph {
	border: solid 1px black;
}
</style>
