<template>
	<div :class="bodyClasses">
		<div :class="sectionMainClasses">
			<component
				:is="childBlockComponent"
				v-for="(childSettings, childIndex) in blockChildren"
				:key="childSettings.id"
				:settings="childSettings"
				:index="childIndex"
				:nested-level="nestedLevelNext"
				:is-editable="isEditable"
				:is-first-child="!childIndex"
				:is-last-child="childIndex >= blockChildren.length - 1"
				:parent-is-horizontal="true"
				@select="selectFromChild"
				@insert="insertFromChild"
			/>
		</div>
	</div>
</template>

<script>
// this needs to be converted over to use the row/col system in chota
export default {
	name: "CoreBlockColumns",
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
		bodyClasses() {
			const classList = ["block__body"];
			if (!this.hasChildren) {
				classList.push("block__body--empty");
			}
			return classList;
		},
		blockChildren() {
			return this.settings?.blocks || [];
		},
		blockChildrenByColumnCount() {
			const countDiff = this.columnCount - this.blockChildrenCount;
			if (countDiff === 0) {
				return this.blockChildren;
			}
			const adjustedChildren = [...this.blockChildren];
			if (countDiff < 0) {
				adjustedChildren.length = this.columnCount;
			}
			if (countDiff > 0) {
				for (let i = 0; i < countDiff; i++) {
					adjustedChildren.push({
						type: "rows",
						id: `${this.blockId}_c${i}`, // assures this is a unique id
						blocks: [
							{
								type: null,
								id: `${this.blockId}_c${i}_0`, // assures this is a unique id
							},
						],
					});
				}
			}
			return adjustedChildren;
		},
		blockChildrenCount() {
			return this.blockChildren.length;
		},
		blockId() {
			return this.settings.id;
		},
		blockVariantLayout() {
			return this.settings?.variants?.layout || "rows";
		},
		columnCount() {
			let count = this.settings["column-count"];
			if (count < 2) {
				count = 2;
			}
			if (count > 6) {
				count = 6;
			}
			return count;
		},
		nestedLevelNext() {
			return this.nestedLevel + 1;
		},
		sectionMainClasses() {
			const classList = ["block__container block__section_main"];
			let columnCount = this.settings["column-count"];
			if (columnCount < 2) {
				columnCount = 2;
			}
			if (columnCount > 6) {
				columnCount = 6;
			}
			classList.push(`block__section_main--${columnCount}_column`);
			return classList;
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
	==
	NOTE: Chota has a column system already sorted out
		(like .col-3, .col-9)
		When there is time it would probably make sense
		to use that instead of all this CSS below
*/
.block_columns {
	text-align: inherit;
	justify-content: inherit;
	& > .block__body.block__body--empty {
		pointer-events: all;
		min-height: 3rem;
	}
	/* constrains background to container width */
	&.block_columns_contained > .block__body {
		max-width: var(--_core__container_max-width);
	}
	/* extends contents to full body width */
	&.block_columns_full-bleed > .block__body > .block__container {
		max-width: var(--_core__body_max-width);
	}
	&.block_columns_offset-left {
		@include breakpoint_up(lg) {
			& > .block__body > .block__section_main {
				&.block__section_main--2_column {
					& > .block {
						&:nth-child(1) {
							width: 65%;
						}
						&:nth-child(2) {
							width: 35%;
						}
					}
				}
			}
		}
	}
	&.block_columns_offset-right {
		@include breakpoint_up(lg) {
			& > .block__body > .block__section_main {
				&.block__section_main--2_column {
					& > .block {
						&:nth-child(1) {
							width: 35%;
						}
						&:nth-child(2) {
							width: 65%;
						}
					}
				}
			}
		}
	}
	@include breakpoint_up(lg) {
		& > .block__body > .block__section_main {
			display: flex;
			position: relative;
			flex-direction: row;
			margin: 0 auto;
			& > .block {
				height: auto;
				padding: 0 calc(var(--_core__container_gutter) / 2);
			}
			&.block__section_main--2_column {
				& > .block {
					width: 50%;
					&:first-child {
						padding-left: 0;
					}
					&:last-child {
						padding-right: 0;
					}
					&:nth-child(1n + 3) {
						display: none;
					}
				}
			}
			&.block__section_main--3_column {
				& > .block {
					width: 33.33%;
					&:nth-child(1n + 4) {
						display: none;
					}
				}
			}
			&.block__section_main--4_column {
				& > .block {
					width: 25%;
					&:nth-child(1n + 5) {
						display: none;
					}
				}
			}
			&.block__section_main--5_column {
				& > .block {
					width: 20%;
					&:nth-child(1n + 6) {
						display: none;
					}
				}
			}
			&.block__section_main--6_column {
				& > .block {
					width: 16.66%;
					&:nth-child(1n + 7) {
						display: none;
					}
				}
			}
		}
		&.block_columns_y-center > .block__body > .block__section_main {
			align-items: center;
		}
		&.block_columns_y-bottom > .block__body > .block__section_main {
			align-items: flex-end;
		}
		&.block_columns_reverse > .block__body > .block__section_main {
			flex-direction: row-reverse;
		}
	}

	& > .block__body {
		background: var(--_core__container_background);
	}

	&.block_columns_transparent > .block__body {
		--_core__container_background: transparent;
	}

	&.block_columns_white > .block__body {
		--_core__container_background: var(--core__color_white);
		--_core__container_color: var(--core__color_primary);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__header_color);
	}

	&.block_columns_light > .block__body {
		--_core__container_background: var(--core__color_grey--20);
		--_core__container_background: var(--core__color_light);
		--_core__container_color: var(--core__color_dark);
		--_core__header_color: var(--core__color_primary);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_columns_primary > .block__body {
		--_core__container_background: var(--core__color_primary);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}

	&.block_columns_dark > .block__body {
		--_core__container_background: var(--core__color_dark);
		--_core__container_color: var(--core__color_light-alt);
		--_core__header_color: var(--core__color_light);
		--_core__overline_color: var(--_core__header_color);
		--_core__subline_color: var(--_core__header_color);
		--_core__disclosure_color: var(--_core__container_color);
	}
}
</style>
