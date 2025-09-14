<template>
	<div :class="bodyClasses">
		<div
			:class="sectionMainClasses"
			:style="[blockBackgroundImage]"
		>
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
export default {
	name: "CoreBlockGrid",
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
		blockChildrenCount() {
			return this.blockChildren.length;
		},
		blockId() {
			return this.settings.id;
		},
		blockVariantLayout() {
			return this.settings?.variants?.layout || "rows";
		},
		blockBackgroundImage() {
			const bgStyle = {
				"background-image": `url(${this.settings.image})`,
				"background-position": "center",
				"padding-top": "12rem",
			};
			return this.settings.image ? bgStyle : "";
		},
		sectionMainClasses() {
			const classList = ["block__section_main"];
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
		nestedLevelNext() {
			return this.nestedLevel + 1;
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
.block_grid {
	padding: 0.5rem;
	text-align: var(--_core__body_text-align);
	> .block__body.block__body--empty {
		pointer-events: all;
		min-height: 3rem;
	}
	@include breakpoint_up(lg) {
		& > .block__body > .block__section_main {
			display: flex;
			flex-direction: row;
			flex-wrap: wrap;
			justify-content: space-between;
			&.block__section_main--2_column > .block {
				min-width: 50%;
				flex: 1;
			}
			&.block__section_main--3_column > .block {
				min-width: 33.33%;
				flex: 1;
			}
			&.block__section_main--4_column > .block {
				min-width: 25%;
				flex: 1;
			}
			&.block__section_main--5_column > .block {
				min-width: 20%;
				flex: 1;
			}
			&.block__section_main--6_column > .block {
				min-width: 16.66%;
				flex: 1;
			}
		}
		&.block_columns_y-center > .block__body > .block__section_main {
			align-items: center;
		}
		&.block_columns_y-bottom > .block__body > .block__section_main {
			align-items: flex-end;
		}
	}
}
</style>
