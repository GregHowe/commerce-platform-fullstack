<template>
	<div class="mt-6">
		<v-row
			justify="space-between"
			align="center"
		>
			<v-col
				cols="8"
				class="flex-nowrap"
			>
				<h1
					v-if="heading"
					class="text-capitalize"
				>
					{{ heading }}
				</h1>
				<p
					v-if="subHeading"
					class="text--secondary"
				>
					{{ subHeading }}
				</p>
				<ForgeButton
					v-if="isAssetPage"
					text
					class="primary--text"
					nuxt
					plain
					@click="goToPreviousPage"
					><v-icon>mdi-arrow-left</v-icon>Back</ForgeButton
				>
				<EditorString
					v-if="isAddNewPage"
					class="primary--text"
					outlined
					prepend-inner-icon="mdi-magnify"
					hint="Type to search"
				/>
			</v-col>
			<v-col cols="4">
				<slot />
			</v-col>
		</v-row>
	</div>
</template>

<script>
export default {
	props: {
		heading: {
			type: String,
			default: "",
		},
		subHeading: {
			type: String,
			default: "",
		},
	},
	computed: {
		isAssetPage() {
			return this.$route.params.id ? true : null;
		},
		isAddNewPage() {
			return this.$route.name === "library-content-new" ? true : null;
		},
	},
	methods: {
		goToPreviousPage() {
			this.$router.go(-1);
		},
	},
};
</script>
