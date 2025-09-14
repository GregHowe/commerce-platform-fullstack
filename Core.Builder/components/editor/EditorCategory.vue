<template>
	<div class="editor-category">
		<template v-if="!isCustom">
			<EditorSelect
				v-model="localizedValue"
				v-bind="$attrs"
				:label="label || 'Choose a Category'"
				:items="selectCategories"
				item-text="title"
				item-value="id"
				clearable
			/>
			<ForgeButton @click="showCustom"> Add Category </ForgeButton>
		</template>
		<template v-else>
			<EditorString
				v-model="customValue"
				v-bind="$attrs"
				:label="`${label}: Custom` || 'Add a Custom Category'"
			/>
			<ForgeButton @click="submitCustom">Submit</ForgeButton>
			<ForgeButton @click="cancelCustom">Cancel</ForgeButton>
		</template>
	</div>
</template>

<script>
import { mapActions, mapState } from "vuex";
import { kebabCase as _kebabCase } from "lodash";
export default {
	name: "EditorCategory",
	inheritAttrs: false,
	props: {
		value: {
			type: Number,
			default: null,
		},
		label: {
			type: String,
			default: null,
		},
	},
	data() {
		return {
			isCustom: false,
			customValue: "",
		};
	},
	computed: {
		...mapState("library", ["categories"]),
		selectCategories() {
			return this.categories || [];
		},
		localizedValue: {
			get() {
				return this.value;
			},
			set(inputValue) {
				this.emitInput(inputValue);
			},
		},
	},
	methods: {
		...mapActions("library", ["createCategory"]),
		emitInput(inputValue) {
			if (!this.$attrs.disabled && !this.$attrs.readonly)
				this.$emit("input", inputValue);
		},
		showCustom() {
			//if (!this.$attrs.disabled && !this.$attrs.readonly)
			this.isCustom = true;
		},
		cancelCustom() {
			this.isCustom = false;
		},
		async submitCustom() {
			this.$store;
			this.$router;
			this.$filters;
			if (!this.$attrs.disabled && !this.$attrs.readonly) {
				if (!this.customValue) {
					return;
				}
				const newCategory = await this.createCategory({
					handle: _kebabCase(this.customValue),
					title: this.customValue,
				});
				this.localizedValue = newCategory.id;
				this.isCustom = false;
			}
		},
	},
};
</script>
