<template>
	<div class="editor editor_block-variant">
		<v-list>
			<v-list-item
				v-for="variantGroup in variantGroupsByType"
				:key="variantGroup.key"
				density="compact"
			>
				<EditorSelect
					v-if="variantGroup.options"
					v-model="inputValue[variantGroup.key]"
					:clearable="true"
					:label="variantGroup.label"
					:items="variantGroup.options"
					density="compact"
					@input="
						(newVal) => {
							setVariant(variantGroup.key, newVal);
						}
					"
				/>
				<EditorBoolean
					v-else
					v-model="inputValue[variantGroup.key]"
					:label="variantGroup.label"
					@input="
						(newVal) => {
							setVariant(variantGroup.key, newVal);
						}
					"
				/>
			</v-list-item>
			<v-list-item
				v-for="variantGroup in variantGroupsGlobal"
				:key="variantGroup.key"
				density="compact"
			>
				<EditorSelect
					v-if="variantGroup.options"
					v-model="inputValue[variantGroup.key]"
					:clearable="true"
					:label="variantGroup.label"
					:items="variantGroup.options"
					density="compact"
					@input="
						(newVal) => {
							setVariant(variantGroup.key, newVal);
						}
					"
				/>
				<EditorBoolean
					v-else
					v-model="inputValue[variantGroup.key]"
					:label="variantGroup.label"
					@input="
						(newVal) => {
							setVariant(variantGroup.key, newVal);
						}
					"
				/>
			</v-list-item>
			<v-list-item v-if="canConstructBlocks">
				<v-list-item-content>
					<v-textarea
						v-model="customClasses"
						dense
						class="d-block"
						label="Custom Classes"
						hint="These classes will be added to the outermost .block element. Tip: use animate.css classes!"
					/>
				</v-list-item-content>
			</v-list-item>
		</v-list>
	</div>
</template>

<script>
import { get } from "http";
import schema from "~/schemas/block.json";
import EditorBoolean from "./EditorBoolean.vue";
export default {
	name: "EditorBlockVariant",
	components: { EditorBoolean },
	props: {
		blockId: {
			type: String,
			required: true,
		},
		blockType: {
			type: String,
			required: true,
		},
		value: {
			type: Object,
			default: () => {
				return {};
			},
		},
	},
	computed: {
		inputValue() {
			return {
				...this.value,
			};
		},
		blockTypeSchema() {
			return this.blockType ? schema.types[this.blockType] : null;
		},
		// returns all the possible variants
		// based on the block type
		variantGroupsByType() {
			return this.blockTypeSchema?.variants || [];
		},
		variantGroupsGlobal() {
			return schema.variants;
		},
		customClasses: {
			get() {
				return this.value?._custom || "";
			},
			set(newValue) {
				this.setVariant("_custom", newValue);
			},
		},
		canConstructBlocks() {
			return this.$store.getters["user/permissions"].includes(
				"constructblocks"
			);
		},
	},
	methods: {
		setVariant(key, newVal) {
			const variants = {
				...this.value,
			};
			variants[key] = newVal;
			this.$emit("input", variants);
		},
	},
};
</script>
