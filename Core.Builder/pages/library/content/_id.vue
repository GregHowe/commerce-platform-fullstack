<template>
	<div>
		<v-alert
			v-if="alert.type"
			:type="alert.type"
			:color="alert.type === 'success' ? 'primary' : 'red'"
			app
			class="mt-3"
			dismissable
		>
			{{ alert.content }}
		</v-alert>
		<ContentLibraryHeader
			:heading="`Edit ${assetType}`"
			sub-heading="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed
					do eiusmod tempor incididunt ut labore et dolore magna
					aliqua."
		>
			<ForgeButton
				text
				class="primary white--text mr-3"
				@click="handleSave"
			>
				Save
			</ForgeButton>
			<ForgeButton
				text
				class="primary white--text"
			>
				Submit to Content Library
			</ForgeButton>
		</ContentLibraryHeader>

		<ContentLibraryForm
			:content-type="assetType"
			:content-item="assetForm"
			:update-method="setAssetTypeSetting"
		/>
	</div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import schema from "~/schemas/library.json";

export default {
	data() {
		return {
			assetForm: {},
			alert: {
				content: "",
				type: "",
			},
		};
	},
	computed: {
		...mapGetters("library", ["getContentItem"]),
		...mapGetters("validation", [
			"validationFailedMessages",
			"validationFailedAny",
		]),
		contentItem() {
			return this.getContentItem(this.$route.params.id);
		},
		assetType() {
			return this.contentItem?.type;
		},
	},
	mounted() {
		Object.entries(this.contentItem).forEach((entry) => {
			this.$set(this.assetForm, entry[0], entry[1]);
		});
		// each library settings item needed to be made reactive
		// including the settings object itself if it didn't exist
		if (this.contentItem.settings) {
			this.$set(this.assetForm, "settings", {});
			Object.entries(this.contentItem.settings).forEach((entry) => {
				this.$set(this.assetForm.settings, entry[0], entry[1]);
			});
		}
	},
	methods: {
		...mapActions("library", ["updateContent"]),
		schemaIncludesSetting(key) {
			return !!schema.types[this.assetType].fields.filter(
				(type) => type.key === key
			).length;
		},
		setAssetTypeSetting({ key, value }) {
			let obj = this.assetForm;
			// the library schema type level object values go into a
			// "settings" object when they are saved to the database
			// and should be updated there.
			if (
				this.assetForm.settings[key] ||
				this.schemaIncludesSetting(key)
			) {
				obj = this.assetForm.settings;
			}
			this.$set(obj, key, value);
		},
		async handleSave() {
			if (this.validationFailedAny) {
				this.alert = {
					content: this.validationFailedMessages.toString(),
					type: "error",
				};
			} else {
				const formPackage = {
					...this.assetForm,
					id: this.$route.params.id,
				};
				const response = await this.updateContent(formPackage);
				if (!response.id) {
					this.alert = {
						content: "There was a problem updating this asset.",
						type: "error",
					};
				} else {
					await this.$router.push({ name: "library-content" });
				}
			}
		},
	},
};
</script>
