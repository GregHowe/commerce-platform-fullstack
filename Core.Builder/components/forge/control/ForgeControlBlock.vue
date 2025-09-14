<template>
	<CoreBlock
		:class="blockClasses"
		:index="index"
		:settings="settings"
		:is-editable="true"
		@select="select"
		@insert="insert"
	>
		<template slot="controls">
			<div
				v-if="hasControls"
				class="forge__controls_block"
				@mouseenter="highlight"
			>
				<ForgeControlBlockInsert
					v-if="isSelected"
					:is-horizontal="parentIsHorizontal"
					:has-children="hasChildren"
					:block-type="settings.type"
					:nested-level="nestedLevel"
					@insert="insertIntoParent"
					@choose="chooseIntoParent"
				/>
				<ForgeControlBlockSelect
					v-if="!isSelected"
					@select="selectIntoParent"
				/>
				<ForgeControlBlockControls
					v-if="isSelected"
					:block-type="blockType"
					:block-id="blockId"
					:is-horizontal="parentIsHorizontal"
					:is-first-child="isFirstChild"
					:is-last-child="isLastChild"
					:copied-block="hasCopiedBlock"
					@movedown="movedown"
					@moveup="moveup"
					@remove="remove"
					@moveleft="moveleft"
					@moveright="moveright"
					@copy-block="copyBlock"
					@cut-block="cutBlock"
					@paste-block="pasteBlock"
				/>
			</div>
		</template>
	</CoreBlock>
</template>

<script>
import schema from "~/schemas/block.json";
import blockHelper from "~/helpers/blocks.js";
import { cloneDeep as _cloneDeep } from "lodash";
import { mapMutations, mapState, mapActions } from "vuex";
export default {
	name: "ForgeControlBlock",
	props: {
		index: {
			type: Number,
			default: 0,
		},
		isFirstChild: {
			type: Boolean,
			default: false,
		},
		isLastChild: {
			type: Boolean,
			default: false,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
		settings: {
			type: Object,
			required: true,
		},
		parentIsHorizontal: {
			type: Boolean,
			default: false,
		},
	},
	computed: {
		...mapState("site", [
			"selectedBlockIds",
			"highlightedBlockIds",
			"selectedBlockId",
			"workingBlock",
			"workingPage",
			"copiedBlock",
		]),

		blockId() {
			return this.settings.id;
		},
		blockType() {
			return this.settings.type;
		},
		blockClasses() {
			const classList = [];
			if (this.isSelected) {
				classList.push("block--selected");
			}
			if (this.isUnselected) {
				classList.push("block--unselected");
			}
			if (this.isHighlighted) {
				classList.push("block--highlighted");
			}
			if (this.isHighlightedDeeply) {
				classList.push("block--highlighted-deeply");
			}
			if (this.isUnhighlighted) {
				classList.push("block--unhighlighted");
			}
			if (this.isSelectedDeeply) {
				classList.push("block--selected-deeply");
			}
			if (this.isSelectedDeeply) {
				classList.push("block--selected-deeply");
			}
			return classList;
		},
		blockTypeSchema() {
			if (!this.blockType) {
				return null;
			}
			return schema.types[this.blockType];
		},
		blockTypeRequiresChild() {
			return this.blockTypeSchema?.requireChild || false;
		},
		isSelected() {
			return (
				this.selectedBlockId && this.selectedBlockId === this.blockId
			);
		},
		isSelectedDeeply() {
			return (
				this.blockId &&
				this.selectedBlockIds.length &&
				!this.isSelected &&
				this.selectedBlockIds.indexOf(this.blockId) >= 0
			);
		},
		isUnselected() {
			return this.selectedBlockId && !this.isSelected;
		},
		isHighlighted() {
			return (
				this.highlightedBlockIds.length &&
				this.highlightedBlockIds[
					this.highlightedBlockIds.length - 1
				] === this.blockId
			);
		},
		isHighlightedDeeply() {
			return (
				this.highlightedBlockIds.length &&
				this.highlightedBlockIds.indexOf(this.blockId) > -1 &&
				!this.isHighlighted
			);
		},
		isUnhighlighted() {
			return (
				this.highlightedBlockIds.length &&
				this.highlightedBlockIds.indexOf(this.blockId) === -1
			);
		},
		hasControls() {
			if (!this.blockId) {
				return false;
			}
			if (!this.blockType) {
				return true;
			}
			return (
				this.blockTypeSchema?.hasControls !== false &&
				!this.settings.locked
			);
		},
		hasChildren() {
			return Boolean(this.settings?.blocks?.length);
		},
		hasCopiedBlock() {
			return this.copiedBlock ? true : false;
		},
	},
	methods: {
		...mapMutations("site", [
			"setHighlightedBlockIds",
			"extendWorkingBlock",
			"setWorkingPageBlocks",
			"setCopiedBlock",
			"setCopiedBlockAsWorkingBlock",
		]),
		...mapActions("site", ["selectBlockIds", "insertBlock"]),
		highlight(target = null) {
			this.setHighlightedBlockIds({ blockId: this.blockId, target });
		},

		insert({ settings, position }) {
			const adjustedSettings = {
				...settings,
			};
			this.$emit("insert", { settings: adjustedSettings, position });
		},
		// this only happens on the top level
		// so does not need to be chained upwards
		chooseIntoParent({ placement, type }) {
			this.$emit("choose", {
				position: placement > 0 ? this.index + 1 : this.index,
				type,
			});
		},
		insertIntoParent({ placement, settings }) {
			const payload = {
				settings,
				position: [],
			};
			if (placement < 0) {
				payload.position.push(this.index); // insert sibling before this block
			} else if (placement > 0) {
				payload.position.push(this.index + 1); // insert sibling after this block
			} else {
				payload.position.push(this.index);
				payload.position.push(0); // insert child into this block
			}

			this.insert(payload);
		},

		select(blockIds = []) {
			this.$emit("select", blockIds);
		},

		selectIntoParent() {
			const target = this.blockId ? [this.blockId] : [];
			this.select(target);
		},
		movedown() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksUpdated = blockHelper.move(
				blocksCloned,
				this.workingBlock.id,
				"down"
			);
			this.setWorkingPageBlocks(blocksUpdated);
		},
		moveup() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksUpdated = blockHelper.move(
				blocksCloned,
				this.workingBlock.id,
				"up"
			);

			this.setWorkingPageBlocks(blocksUpdated);
		},
		moveright() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksUpdated = blockHelper.move(
				blocksCloned,
				this.workingBlock.id,
				"right"
			);
			this.setWorkingPageBlocks(blocksUpdated);
		},
		moveleft() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksUpdated = blockHelper.move(
				blocksCloned,
				this.workingBlock.id,
				"left"
			);
			this.setWorkingPageBlocks(blocksUpdated);
		},
		remove() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksUpdated = blockHelper.remove(
				blocksCloned,
				this.workingBlock.id
			);
			this.setWorkingPageBlocks(blocksUpdated);
			this.selectBlockIds();
		},
		cutBlock() {
			const blocksCloned = _cloneDeep(this.workingPage.blocks);
			const blocksCopied = _cloneDeep(this.workingBlock);
			this.setCopiedBlock(blocksCopied);
			const blocksUpdated = blockHelper.remove(
				blocksCloned,
				this.workingBlock.id
			);
			this.setWorkingPageBlocks(blocksUpdated);
			this.selectBlockIds();
		},
		copyBlock() {
			const blocksCopied = _cloneDeep(this.workingBlock);
			this.setCopiedBlock(blocksCopied);
		},
		pasteBlock() {
			this.setCopiedBlockAsWorkingBlock();
			// this.setWorkingPageBlocks();
			this.selectBlockIds();
		},
	},
};
</script>

<style lang="scss">
// styles universal to blocks (will refactor into CoreLibrary)

/* handling z-index things separately because it is easier to interpret this way */
.block {
	z-index: 101;
	/* the real core block */
	& > .block__body {
		/* the real core block's body */
		z-index: 201;
	}
	& > .forge__controls_block,
	& > .forge__controls_block_controller {
		/* buttons that are on top of the block in the builder only */
		z-index: 301;
	}

	&.block--selected-deeply {
		/* the selected block is INSIDE this block */
		& > .block__body {
			z-index: 301;
		}
		& > .forge__controls_block,
		& > .forge__controls_block_controller {
			z-index: 201;
		}
	}
	&.block--selected {
		z-index: 501;
	}
}

.block {
	/*
			handles all the indicators of the blocks
			selection/hover status in the builder
		*/
	cursor: pointer;
	border: 0; // applying a border causes a 1px shift in the layout
	outline: 1px solid transparent;
	opacity: 1;
	transition: opacity 0.4s;
	&.block--highlighted {
		/* this exact block is highlighted */
		box-shadow: inset 0 0 1px 1px var(--v-info-lighten5);
		outline: 1px dotted var(--v-anchor-base);
		outline-offset: -1px;
		.block.block--unhighlighted {
			/* nested blocks */
			opacity: 1;
		}
	}
	&.block--unhighlighted {
		/* other block is highlighted */
		opacity: 0.8;
	}
	/* the real core block */
	.block__body {
		/* block interaction */
		pointer-events: none;
	}
	.forge__controls_block {
		position: absolute;
		top: 0;
		right: 0;
		bottom: 0;
		left: 0;
		max-width: var(--_core__container_max-width);
		pointer-events: none;
		height: 100%;
	}

	&.block--selected {
		cursor: default;
		outline: solid 1px #2b9ce0;
		z-index: 99999;
		opacity: 1 !important;
	}
	&.block--selected-deeply {
		outline: dotted 1px green;
		pointer-events: none;
		opacity: 1 !important;
		.block {
			/* nested */
			&.block--selected {
				outline: 1px solid #2b9ce0 !important;
			}
			&.block--unselected {
				opacity: 0.6;
			}
			&.block--highlighted {
				opacity: 0.8;
			}
			&.block--selected .block.block--unselected {
				opacity: 1;
			}
		}
		& > .block__body > .block__section_main > .block {
			/* enable interaction on nested blocks */
			pointer-events: all;
		}
	}

	&.block_container.block--highlighted-deeply {
		/* special case of the container block that has a nested block highlighted inside of itself */
		opacity: 1;
	}
}
</style>
