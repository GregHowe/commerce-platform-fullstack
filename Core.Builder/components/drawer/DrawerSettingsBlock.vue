<template>
	<div class="drawer__body ma-0 pa-0">
		<v-list class="ma-0 pa-0">
			<v-list-item three-item>
				<v-list-item-content>
					<v-list-item-title class="text-h5 mb-1">
						<v-icon
							color="orange"
							class="mr-2"
							>mdi-cube</v-icon
						>{{ blockName || "New" }}
					</v-list-item-title>
					<v-list-item-subtitle>
						Modify block settings and styles.
					</v-list-item-subtitle>
				</v-list-item-content>
			</v-list-item>
		</v-list>
		<template v-if="workingBlock">
			<EditorBlockType
				v-if="!workingBlock.type"
				:value="workingBlock.type"
				:depth="blockDepth"
				@input="setBlockType"
			/>
		</template>
		<v-tabs
			v-model="activeTabIndex"
			background-color="blue lighten-5"
			centered
			icons-and-text
			dense
			color="blue"
			:show-arrows="false"
			style="border-top: solid 1px #aaa"
		>
			<v-tab>
				<v-icon class="mt-2">mdi-filmstrip</v-icon>
				Content
			</v-tab>
			<v-tab>
				<v-icon class="mt-2">mdi-palette</v-icon>
				Style
			</v-tab>
		</v-tabs>
		<v-window
			v-model="activeTabIndex"
			class="white"
		>
			<v-window-item :value="0">
				<v-list
					v-if="schemaFields"
					dense
					class="rounded"
				>
					<v-list-item
						v-for="schemaField in schemaFields"
						v-show="isSchemaFieldShowing(schemaField)"
						:key="schemaField.key"
						rounded
						dense
						class="my-2"
						><!-- pass _settings to private tab -->
						<component
							:is="getEditorBySchema(schemaField.type)"
							:label="schemaField.label"
							:hint="schemaField.hint"
							:provider-name="schemaField.label"
							:rules="schemaField.rules"
							:accept-types="schemaField.acceptTypes"
							:value="getWorkingBlockSetting(schemaField.key)"
							v-bind="schemaField.attrs || null"
							:fetch-editor-method="getEditorBySchema"
							:type="schemaField.type"
							@input="
								(newValue) => {
									setWorkingBlockSetting({
										key: schemaField.key,
										value: newValue,
									});
								}
							"
						/>
					</v-list-item>
				</v-list>
			</v-window-item>
			<v-window-item :value="1">
				<v-list
					dense
					class="rounded"
				>
					<EditorBlockVariant
						:value="workingBlock.variants"
						:block-id="blockId"
						:block-type="blockType"
						@input="setVariants"
					/>
				</v-list>
			</v-window-item>
		</v-window>
	</div>
</template>
<script>
import {
	get as _get,
	set as _set,
	upperFirst as _upperFirst,
	cloneDeep as _cloneDeep,
} from "lodash";
import schema from "~/schemas/block.json";
import blockHelper from "~/helpers/blocks.js";
import editorHelper from "~/helpers/editors.js";
import { mapActions, mapMutations, mapState } from "vuex";

export default {
	data() {
		return {
			activeTabIndex: 0,
		};
	},
	computed: {
		...mapState("site", [
			"workingPage",
			"workingBlock",
			"selectedSiteId",
			"workingSite",
			"copiedBlock",
		]),
		blockType() {
			if (this.workingBlock) {
				return this.workingBlock.type;
			}
			return null;
		},
		blockDepth() {
			// added this because blocks at the '0' level of depth
			// can only be row, column, or grid
			// so passing depth to the BlockTypeEditor was necessary
			const indexPath = blockHelper.findIndexes(
				this.workingPage.blocks,
				this.blockId
			);
			return indexPath.length - 1;
		},
		blockId() {
			return this.workingBlock.id;
		},
		// returns all the extra fields
		// a block type has based on the schema
		blockTypeSchema() {
			if (!this.blockType) {
				return null;
			}
			return schema.types[this.blockType];
		},
		blockTypeSchemaFields() {
			if (!this.blockTypeSchema) {
				return [];
			}
			return this.blockTypeSchema.fields || [];
		},
		schemaFields() {
			return [...schema.fields, ...(this.blockTypeSchema?.fields || [])];
		},
		schemaVariants() {
			return this.blockTypeSchema?.variants;
		},
		blockName() {
			return _upperFirst(this.blockType);
		},
		workingPageTitle() {
			return this.workingPage?.title || "";
		},
	},
	methods: {
		...mapActions("site", ["selectBlockIds", "removeBlock"]),
		...mapMutations("site", [
			"extendWorkingBlock",
			"setWorkingBlockSetting",
			"setWorkingPageBlocks",
		]),
		...mapMutations("interface", ["setMaximizeSettings"]),
		getWorkingBlockSetting(key) {
			return _get(this.workingBlock, key);
		},
		getEditorBySchema: editorHelper.getEditorBySchema,
		setVariants(newVal) {
			this.extendWorkingBlock({
				variants: newVal,
			});
		},
		setBlockStyle(newValue) {
			this.setWorkingBlockSetting({
				key: "style",
				value: newValue,
			});
		},
		setBlockType(typeKey) {
			this.extendWorkingBlock({
				type: typeKey,
			});
			if (this.schemaFields) {
				const defaultValues = {};
				this.schemaFields.forEach((schemaField) => {
					if (
						!schemaField.readonly &&
						!this.getWorkingBlockSetting(schemaField.key) // not already set
					) {
						defaultValues[schemaField.key] =
							schemaField.default || null;
					}
				});
				if (
					this.blockTypeSchema?.requireChild &&
					!this.getWorkingBlockSetting("blocks") // not already set
				) {
					defaultValues.blocks = [blockHelper.getBlank()];
					if (typeKey === "toggle") {
						// toggle should have 2 child blocks
						defaultValues.blocks.push(blockHelper.getBlank());
					}
				}
				this.extendWorkingBlock(defaultValues);
				const variantValues = {};
				if (this.schemaVariants) {
					this.schemaVariants.forEach((schemaVariant) => {
						variantValues[schemaVariant.key] =
							schemaVariant.default || null;
						variantValues.hidden = schemaVariant.hidden || false;
					});
					this.setVariants(variantValues);
				}
			}
		},
		setWorkingBlockSetting({ key, value }) {
			const update = {};
			_set(update, key, value);
			this.extendWorkingBlock(update);
		},
		isSchemaFieldShowing(schemaField) {
			let isShowing = true;
			if (schemaField.hidden) {
				isShowing = false;
			}
			// if this field has a requireSettings property
			// it should be an array of keys that point to other
			// fields which must have specific values for this field to show up
			if (schemaField.requireSettings) {
				schemaField.requireSettings.forEach((item) => {
					const currentValue = this.workingBlock[item.key];
					const acceptableValues = item.values;
					if (acceptableValues.indexOf(currentValue) === -1) {
						isShowing = false; // current value is not found within the acceptable values defined in schema
					}
				});
			}
			return isShowing;
		},
	},
};
</script>
<style lang="scss">
.drawer__body {
	&__group-container {
		margin: 0 0.7rem;
	}
}
</style>
