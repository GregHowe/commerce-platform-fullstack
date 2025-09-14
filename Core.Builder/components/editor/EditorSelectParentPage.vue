<template>
	<div class="editor editor-select-parent">
		<EditorSelect
			:value="selectedPage"
			:items="selectablePages"
			item-text="title"
			:label="label"
			item-value="id"
			clearable
			clear-icon="$clear"
			@input="emitSelectedPage"
		/>
	</div>
</template>

<script>
import { mapState } from "vuex";
export default {
	name: "EditorSelectParentPage",
	inheritAttrs: false,
	props: {
		value: {
			type: Number,
			default: null,
		},
		label: {
			type: String,
			default: "",
		},
	},
	computed: {
		...mapState("site", ["selectedPageId", "workingSite"]),
		selectablePages() {
			if (!this.workingSite) {
				return [];
			}
			return this.workingSite.pages.filter((page) => {
				if (!page.parentPageId) {
					return page.id !== this.selectedPageId;
				}
			});
		},
		selectedPage() {
			return this.value || "";
		},
	},
	methods: {
		emitSelectedPage(pageId) {
			this.$emit("input", pageId);
		},
	},
};
</script>
