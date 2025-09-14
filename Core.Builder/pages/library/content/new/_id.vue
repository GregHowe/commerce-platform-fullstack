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
			:heading="`Add New ${assetType}`"
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
			:update-method="setAssetTypeSetting"
			:content-item="assetForm"
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
		...mapGetters({
			validationFailedAny: "validation/validationFailedAny",
			validationFailedMessages: "validation/validationFailedMessages",
		}),
		assetType() {
			return this.$route.params.id;
		},
	},
	mounted() {
		const schemaFields = schema.types[this.$route.params.id].fields;
		schemaFields.forEach((schemaField) => {
			this.$set(this.assetForm, schemaField.key, schemaField.default);
		});
	},
	methods: {
		...mapActions("library", ["createContent"]),
		async handleSave() {
			if (this.validationFailedAny) {
				this.alert = {
					content: this.validationFailedMessages.toString(),
					type: "error",
				};
			} else {
				const response = await this.createContent({
					type: this.assetType,
					...this.assetForm,
				});
				if (!response.id) {
					this.alert = {
						content: "There was a problem adding this asset.",
						type: "error",
					};
				} else {
					await this.$router.push({ name: "library-content" });
				}
			}
		},
		setAssetTypeSetting({ key, value }) {
			this.$set(this.assetForm, key, value);
		},
	},
};
</script>
