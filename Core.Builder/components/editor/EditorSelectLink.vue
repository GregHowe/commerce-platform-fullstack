<template>
	<div class="editor editor-select-relative">
		<label>{{ label }}</label>
		<EditorSelect
			:items="['Page', 'External URL']"
			:value="linkType"
			@input="setLinkType"
		/>
		<EditorUrl
			v-if="linkType === 'External URL'"
			v-model="localizedValue"
			provider-name="External URL"
		/>
		<div v-else>
			<EditorSelect
				:items="selectablePages"
				:value="localizedValue"
				item-text="title"
				item-value="handle"
				placeholder="Select Internal Page"
				@input="emitSelectedUrl"
			/>
			<span class="text-caption">{{ displayedValue }}</span>
		</div>
	</div>
</template>

<script>
import { mapState } from "vuex";
export default {
	name: "EditorSelectLink",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: null,
		},
		label: {
			type: String,
			default: null,
		},
	},
	data() {
		return {
			linkType: "Page",
			displayedValue: "",
		};
	},
	computed: {
		...mapState("site", ["workingSite"]),
		localizedValue: {
			get() {
				return this.getValue(this.value);
			},
			set(inputValue) {
				this.emitSelectedUrl(inputValue);
			},
		},
		selectablePages() {
			return this.workingSite?.pages || [];
		},
	},
	methods: {
		setLinkType(val) {
			this.linkType = val;
		},
		setDisplayedValue(val) {
			this.displayedValue = val;
		},
		getValue(val) {
			if (val) {
				if (val.startsWith("/")) {
					this.setLinkType("Page");
					this.setDisplayedValue(val);
					// get the last (second) item in the url
					// so the value matches the handle in the select
					const secondPath = val.split("/")[2];
					return secondPath || val.replace("/", "");
				} else {
					this.setLinkType("External URL");
				}
				return val;
			}
		},
		emitSelectedUrl(value) {
			let url = value;

			if (this.linkType === "Page") {
				url = `/${value}`; // prepend slash for relative link

				const selectedPage = this.selectablePages.find(
					(page) => page.handle === value
				);
				if (selectedPage?.parentPageId) {
					const parentPage = this.selectablePages.find(
						(page) => page.id === selectedPage.parentPageId
					);
					// keep the url structure as a child page on the link that is created
					url = `/${parentPage.handle}/${value}`;
				}
				this.setDisplayedValue(url);
			}

			this.$emit("input", url);
		},
	},
};
</script>
