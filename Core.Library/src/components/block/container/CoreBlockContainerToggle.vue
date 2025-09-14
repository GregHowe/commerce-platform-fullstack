<template>
	<div class="block_container_toggle__buttons--container">
		<div class="block_container_toggle__buttons">
			<button
				v-for="(toggle, childIndex) in blockChildren"
				:key="toggle.id"
				:class="{ active: activeId === toggle.id }"
				@click="setToggle(toggle.id)"
			>
				{{ toggleLabel(childIndex) }}
			</button>
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockContainerToggle",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		activeId: {
			type: String,
			required: true,
		},
	},
	computed: {
		blockChildren() {
			return this.settings?.blocks || [];
		},
	},
	methods: {
		toggleLabel(toggleIndex) {
			return this.settings[`label-${toggleIndex + 1}`] || "Toggle";
		},
		setToggle(event) {
			this.$emit("set-item", event);
		},
	},
};
</script>

<style lang="scss">
.block_toggle {
	.block__control_nav {
		display: flex;
		list-style: none;
		padding: 0;
		margin: 0;
		.block__control_nav_item {
			color: var(--_core__button_background);
			background: transparent;
			display: block;
			padding: 0.25rem 1.5rem;
			&.block__control_nav_item--active {
				color: var(--_core__button_color);
				background: var(--_core__button_background);
			}
		}
	}
}
</style>
