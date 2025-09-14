<template>
	<div class="drawer__body">
		<v-list-item two-line>
			<v-list-item-content>
				<v-list-item-title class="text-h5 mb-1">
					Site Settings
				</v-list-item-title>
				<v-list-item-subtitle>
					Modify settings for your site.
				</v-list-item-subtitle>
			</v-list-item-content>
		</v-list-item>
		<v-list dense>
			<v-list-item
				v-for="schemaField in schemaFields"
				v-show="isSchemaFieldShowing(schemaField)"
				:key="schemaField.key"
			>
				<component
					:is="getEditorBySchema(schemaField.type)"
					:label="schemaField.label"
					:value="getWorkingSiteSetting(schemaField.key)"
					:rules="schemaField.rules"
					:provider-name="schemaField.label"
					v-bind="schemaField.attrs"
					:fetch-editor-method="getEditorBySchema"
					:type="schemaField.type"
					@input="
						(newValue) => {
							setWorkingSiteSetting({
								key: schemaField.key,
								value: newValue,
							});
						}
					"
				/>
			</v-list-item>
		</v-list>
	</div>
</template>
<script>
import siteSchema from "~/schemas/site.json";
import editorHelper from "~/helpers/editors.js";
import { mapGetters, mapMutations, mapState } from "vuex";
export default {
	computed: {
		...mapState("site", ["workingSite"]),
		...mapGetters("site", ["getWorkingSiteSetting"]),
		schemaFields() {
			return siteSchema.fields.filter((field) => !field.hidden);
		},
	},
	methods: {
		...mapMutations("site", ["setWorkingSiteSetting"]),
		getEditorBySchema: editorHelper.getEditorBySchema,
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
					const currentValue = this.workingSite[item.key];
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
