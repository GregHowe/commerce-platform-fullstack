<template>
	<div class="editor editor-select-home">
		<EditorSelect
			:items="selectablePages"
			:value="localizedValue"
			:label="label"
			item-text="title"
			item-value="id"
			@input="emitSelectedPage"
		/>
	</div>
</template>

<script>
import { mapState } from "vuex";
export default {
	name: "EditorSelectHomePage",
	inheritAttrs: false,
	props: {
		value: {
			type: [String, Number],
			default: null,
		},
		label: {
			type: String,
			default: null,
		},
	},
	computed: {
		...mapState("site", ["workingSite"]),
		localizedValue() {
			if (!this.workingSite?.pages?.length) {
				return null;
			}
			return this.value || this.workingSite?.pages[0];
		},
		selectablePages() {
			if (!this.workingSite) {
				return [];
			}
			return this.workingSite.pages.filter((page) => {
				return page.isActive !== false;
			});
		},
	},
	methods: {
		emitSelectedPage(documentNumber) {
			this.$emit("input", documentNumber);
		},
	},
};
</script>
