<template>
	<div>
		<v-row
			v-for="schemaField in schemaFields"
			:key="schemaField.key"
			class="my-2"
		>
			<v-col cols="6">
				<component
					:is="getEditorBySchema(schemaField.type)"
					:value="schemaFieldValue(schemaField.key)"
					:label="schemaField.label"
					:hint="schemaField.hint"
					:provider-name="schemaField.label"
					:rules="schemaField.rules"
					:items="getSelectItems(schemaField.key)"
					item-text="label"
					item-value="value"
					outlined
					full-width
					v-bind="schemaField.attrs || null"
					@input="
						(newValue) => {
							updateMethod({
								key: schemaField.key,
								value: newValue,
							});
						}
					"
				/>
			</v-col>
		</v-row>
	</div>
</template>

<script>
import { mapState } from "vuex";
import schema from "~/schemas/library.json";
import editorHelper from "~/helpers/editors";

export default {
	name: "ContentLibraryForm",
	props: {
		updateMethod: {
			type: Function,
			default: null,
		},
		contentType: {
			type: String,
			required: true,
		},
		contentItem: {
			type: Object,
			default: null,
		},
	},
	computed: {
		...mapState("library", ["categories"]),
		schemaFields() {
			return [
				// default fields for all types
				...schema.fields.filter((field) => !field.hidden),
				// fields specifically for selected type
				...schema.types[this.contentType].fields.filter(
					(field) => !field.hidden
				),
			];
		},
	},
	methods: {
		getSelectItems(type) {
			switch (type) {
				case "category":
					return this.categories;
			}
		},
		getEditorBySchema: editorHelper.getEditorBySchema,
		schemaFieldValue(value) {
			return (
				this.contentItem?.[value] || this.contentItem?.settings?.[value]
			);
		},
	},
};
</script>
