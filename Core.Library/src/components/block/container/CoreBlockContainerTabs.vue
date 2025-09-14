<template>
	<ul class="block__control block__control_nav">
		<li
			v-for="(tab, tabIndex) in blockChildren"
			:key="tab.id"
			:class="{
				block__control_nav_item: true,
				'block__control_nav_item--active': activeId === tab.id,
			}"
			@click="setTab(tab.id)"
		>
			{{ `Tab ${tabIndex + 1}` }}
		</li>
	</ul>
</template>

<script>
export default {
	name: "CoreBlockContainerTabs",
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
		setTab(event) {
			this.$emit("set-item", event);
		},
	},
};
</script>

<style lang="scss">
.block__control_nav {
	display: flex;
	list-style: none;
	padding: 0;
	margin: 0;
	font-weight: 600;
	text-align: center;
	align-items: center;
	.block__control_nav_item {
		color: var(--core__color_primary);
		border-radius: 50%;
		margin: 0 0.2rem;
		height: 24px;
		width: 24px;
		font-family: var(--core__font_secondary);
		font-style: normal;
		font-weight: 600;
		font-size: 14px;
		line-height: 24px;
		&:hover {
			color: var(--core__color_dark);
			background-color: var(--core__color_inactive-light);
		}
		&.block__control_nav_item--active {
			color: var(--core__color_dark);
			background-color: var(--core__color_primary);
		}
	}
}
.block_container_slim {
	.block__control_nav {
		margin: 40px auto;
		padding: 0;
		.block__control_nav_item {
			color: var(--core__color_primary);
			&:hover {
				color: var(--core__color_white);
				background-color: var(--core__color_secondary);
			}
			&.block__control_nav_item--active {
				color: var(--core__color_white);
				background-color: var(--core__color_primary);
			}
		}
	}
}
</style>
