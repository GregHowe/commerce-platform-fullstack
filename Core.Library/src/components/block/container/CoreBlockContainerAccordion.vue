<template>
	<div
		class="block__control block__control_accordion"
		@click="$emit('toggle-contents')"
	>
		{{ accordionLabel }}
		<CoreIcon
			v-if="isShowingContents"
			icon="circle-minus"
		/>
		<CoreIcon
			v-else
			icon="circle-plus"
		/>
	</div>
</template>

<script>
export default {
	name: "CoreBlockContainerAccordion",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		isShowingContents: {
			type: Boolean,
			default: true,
		},
	},
	computed: {
		accordionLabel() {
			return this.settings.accordionText || "Accordion";
		},
	},
};
</script>

<style lang="scss">
.block_accordion {
	.block__control_nav {
		display: flex;
		list-style: none;
		padding: 0;
		margin: 0;
		.block__control_nav_item {
			color: var(--_core__button_primary-background);
			background: transparent;
			display: block;
			padding: 0.25rem 1.5rem;
			&.block__control_nav_item--active {
				color: var(--_core__button_primary-color);
				background: var(--_core__button_primary-background);
			}
		}
	}
}
</style>
