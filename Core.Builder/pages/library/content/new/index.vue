<template>
	<div>
		<ContentLibraryHeader
			heading="Add New Media"
			sub-heading="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
			eiusmod tempor incididunt ut labore et dolore magna aliqua."
		>
		</ContentLibraryHeader>
		<v-row>
			<v-card
				v-for="schemaField in schemaFields"
				:key="schemaField"
				color="secondary darken-1 "
				class="d-flex flex-column align-center justify-center mx-3 rounded"
				height="300"
				width="375"
			>
				<v-card-title class="text--primary text-capitalize">
					{{ schemaField }}
				</v-card-title>
				<v-card-actions>
					<ForgeButton
						text
						class="primary"
						nuxt
						:to="{
							name: 'library-content-new-id',
							params: { id: schemaField },
						}"
					>
						Create Content
					</ForgeButton>
				</v-card-actions>
			</v-card>
		</v-row>
	</div>
</template>
<script>
import schema from "~/schemas/library.json";

export default {
	layout: "default",
	computed: {
		schemaFields() {
			return Object.keys(schema.types);
		},
	},
};
</script>
