<template>
	<div class="editor editor-main-navigation">
		<v-list-item-title class="text-h6 py-2 mt-2">
			Main Navigation
		</v-list-item-title>
		<v-list dense>
			<EditorMainNavigationTable />
			<v-list-group
				v-for="section in sections"
				:key="section.label"
				:value="false"
				class="editor-main-navigation__group-container"
			>
				<template #activator>
					<CoreIcon
						v-if="section.icon"
						class="mr-2"
						:icon="section.icon"
					/>
					<v-list-item-title v-if="section.label">{{
						section.label
					}}</v-list-item-title>
				</template>
				<v-list-item
					v-for="schemaField in section.fields"
					v-show="isSchemaFieldShowing(schemaField)"
					:key="schemaField.key"
				>
					<component
						:is="fetchEditorMethod(schemaField.type)"
						:label="schemaField.label"
						:value="
							getWorkingSiteSetting(
								`navigation.${schemaField.key}`
							)
						"
						:rules="schemaField.rules"
						:provider-name="`Navigation ${schemaField.label}`"
						v-bind="schemaField.attrs"
						@input="
							(newValue) => {
								setWorkingSiteSetting({
									key: `navigation.${schemaField.key}`,
									value: newValue,
								});
							}
						"
					/>
				</v-list-item>
			</v-list-group>
		</v-list>
	</div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import siteSchema from "~/schemas/site.json";

export default {
	name: "EditorMainNavigation",
	props: {
		label: {
			type: String,
			default: null,
		},
		fetchEditorMethod: {
			type: Function,
			required: true,
		},
	},

	computed: {
		...mapGetters("site", ["getWorkingSiteSetting"]),
		sections() {
			return siteSchema.fields.find(
				(field) => field.type === "navigation"
			).sections;
		},
	},
	methods: {
		...mapMutations("site", ["setWorkingSiteSetting"]),
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
					const currentValue = this.getWorkingSiteSetting(
						`navigation.${item.key}`
					);
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

<style
	lang="scss"
	scoped
>
.editor-main-navigation {
	width: 100%;
	&__group-container {
		background-color: #ffffff;
		margin: 0.7rem 0;
	}
}
</style>
