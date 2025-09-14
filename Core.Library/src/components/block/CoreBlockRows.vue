<template>
	<div
		:class="bodyClasses"
		:style="bodyStyles"
	>
		<div class="block__container block__section_main">
			<component
				:is="childBlockComponent"
				v-for="(childSettings, childIndex) in blockChildren"
				:key="childSettings.id"
				:settings="childSettings"
				:index="childIndex"
				:nested-level="nestedLevelNext"
				:is-editable="isEditable"
				:parent-is-horizontal="false"
				:is-first-child="!childIndex"
				:is-last-child="childIndex >= blockChildren.length - 1"
				@select="selectFromChild"
				@insert="insertFromChild"
			/>
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockRows",
	props: {
		index: {
			type: Number,
			default: 0,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
		settings: {
			type: Object,
			required: true,
		},
		isEditable: {
			type: Boolean,
			default: false,
		},
	},
	computed: {
		blockChildren() {
			return this.settings?.blocks || [];
		},
		blockChildrenCount() {
			return this.blockChildren.length;
		},
		blockId() {
			return this.settings.id;
		},
		blockVariants() {
			return Object.entries(this.settings?.variants || {}).map( ([key, value]) => {
				return {
					key,
					value
				}
			})
		},
		blockVariantLayout() {
			return this.settings?.variants?.layout || "rows";
		},
		hasChildren() {
			return this.settings?.blocks?.length;
		},
		nestedLevelNext() {
			return this.nestedLevel + 1;
		},
		bodyClasses() {
			const classList = ["block__body", ...this.blockVariants.map( (variant) => {
				return `${variant.key}-${variant.value}`
			})];
			if (!this.hasChildren) {
				classList.push("block__body--empty");
			}
			return classList;
		},
		bodyStyles() {
			if (this.settings.backgroundMediaSrc) {
				return {
					background: `url(${this.settings.backgroundMediaSrc})`,
					"background-size": "cover",
					backgroundPosition: "center",
					width: "100%",
					height: this.settings?.rowHeight || "500px",
				};
			}
			return {
				height: this.settings?.rowHeight || 'auto'
				};
		},
		/*
			todo the childBlockComponent property is only useful for the builder
			so it should be separated from this block's logic
		*/
		childBlockComponent() {
			if (this.isEditable) {
				return "ForgeControlBlock";
			}
			return "CoreBlock";
		},
	},
	/*
		todo separate the builder behaviors below (insert/select)
		from the block's concerns,
		since they only apply in the builder
	*/
	methods: {
		insertFromChild({ settings, position }) {
			const positionNested = [this.index, ...position];
			this.$emit("insert", { settings, position: positionNested });
		},
		selectFromChild(blockIds = []) {
			const target = this.blockId
				? [this.blockId, ...blockIds]
				: blockIds;
			this.$emit("select", target);
		},
	},
};
</script>

<style lang="scss">
/*
	Do not use utility classes
	or Vuetify components within
	Core Blocks.
	Define all the styles here
	with CSS, using vars from
	the theme where appropriate.
*/
.block_rows {
	padding: 0;
	background: transparent;
	> .block__body.block__body--empty {
		pointer-events: all;
		min-height: 3rem;
	}

	.align-items-center {
		display: flex;
		align-items: center;
	}

	& > .block__body {
		background: var(--_core__container_background);
	}

	/* constrains background to container width */
	&.block_rows_contained > .block__body {
		max-width: var(--_core__container_max-width);
	}

	/* extends contents to full body width */
	&.block_rows_full-bleed > .block__body > .block__container {
		max-width: var(--_core__body_max-width);
	}

	&.block_rows_transparent > .block__body {
		--_core__container_background: transparent;
	}

	&.block_rows_white > .block__body {
		--_core__container_background: var(--core__color_white);
		--_core__container_color: var(--core__color_primary);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__header_color);
	}

	&.block_rows_light > .block__body {
		--_core__container_background: var(--core__color_grey--20);
		--_core__container_background: var(--core__color_light);
		--_core__container_color: var(--core__color_dark);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_rows_primary > .block__body {
		--_core__container_background: var(--core__color_primary);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_rows_dark > .block__body {
		--_core__container_background: var(--core__color_secondary--dark);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_rows_contained {
		padding: 0 3rem;
	}
}
</style>
