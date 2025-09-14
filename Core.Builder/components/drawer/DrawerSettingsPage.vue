<template>
	<div class="drawer__body">
		<v-list-item two-line>
			<v-list-item-content>
				<v-list-item-title class="text-h5 mb-1">
					<v-icon
						color="blue"
						class="mr-2"
						>mdi-file-document-outline</v-icon
					>{{ workingPageTitle }}
				</v-list-item-title>
				<v-list-item-subtitle>
					Modify settings for your webpage.
				</v-list-item-subtitle>
			</v-list-item-content>
		</v-list-item>
		<v-list dense>
			<v-list-item
				v-for="schemaField in schemaFields"
				:key="schemaField.key"
			>
				<component
					:is="getEditorBySchema(schemaField.type)"
					:label="schemaField.label"
					:value="getWorkingPageSetting(schemaField.key)"
					:provider-name="schemaField.label"
					:rules="schemaField.rules"
					:background-color="backgroundColor(schemaField.attrs)"
					:type="schemaField.type"
					@input="
						(newValue) => {
							setWorkingPageSetting({
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
import pageSchema from "~/schemas/page.json";
import editorHelper from "~/helpers/editors.js";
import { mapGetters, mapMutations, mapState } from "vuex";
export default {
	computed: {
		...mapState("site", ["selectedPageId", "workingPage"]),
		...mapGetters("site", ["getWorkingPageSetting", "hasWorkingChanges"]),
		schemaFields() {
			return pageSchema.fields.filter((field) => !field.hidden);
		},
		workingPageTitle() {
			return this?.workingPage?.title || "";
		},
	},
	methods: {
		...mapMutations("site", ["setWorkingPageSetting"]),
		getEditorBySchema: editorHelper.getEditorBySchema,
		backgroundColor(attrs) {
			return attrs ? attrs["background-color"] : "#ffffff";
		},
	},
};
</script>
