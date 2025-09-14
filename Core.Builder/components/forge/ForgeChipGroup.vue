<template>
	<v-chip-group v-bind="$attrs">
		<ForgeChip
			v-for="chip in chips"
			:key="chip.value"
			:value="chip.value"
			:color="chip.color"
			:text-color="chip.textColor"
			:name="chip.label"
			:icon="chip.icon"
			@click="$emit('input', chip.value)"
		/>
	</v-chip-group>
</template>

<script>
export default {
	name: "ChipGroup",
	props: {
		chips: {
			type: Array,
			default: null,
		},
	},
};
</script>

<style></style>
