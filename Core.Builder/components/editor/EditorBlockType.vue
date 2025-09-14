<template>
	<div class="editor editor_block-type">
		<v-list dense>
			<v-divider></v-divider>
			<v-list-item
				v-for="(type, typeKey) in blockTypeList"
				:key="typeKey"
			>
				<v-btn @click="emitInput(typeKey)">
					set to {{ typeKey }}
				</v-btn>
			</v-list-item>
		</v-list>
	</div>
</template>

<script>
import { pickBy as _pickBy } from "lodash";
import schema from "~/schemas/block.json";
export default {
	name: "EditorBlockType",
	props: {
		depth: {
			type: Number,
			default: 0,
		},
		value: {
			type: String,
			default: null,
		},
	},
	computed: {
		blockTypeList() {
			if (!this.depth) {
				return _pickBy(schema.types, (type) => {
					return type.isPageLevel;
				});
			}
			return _pickBy(schema.types, (type) => {
				return !type.hidden;
			});
		},
		workingValue() {
			return this.value;
		},
	},
	methods: {
		emitInput(inputValue) {
			this.$emit("input", inputValue);
		},
	},
};
</script>
