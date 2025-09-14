<template>
	<div class="index-content">
		<v-card
			flat
			:class="contentCardClasses"
		>
			<v-row>
				<v-img
					contain
					class="index-content-card__image"
					:src="content.src"
					lazy-src="/img/content-placeholder.png"
				/>
			</v-row>
			<div>
				<v-card-title> {{ content.title }}</v-card-title>
				<v-card-subtitle> {{ content.description }}</v-card-subtitle>
			</div>
			<div class="index-content-card__button-container">
				<ForgeButton
					text
					class="primary"
					:to="{
						name: 'library-content-id',
						params: { id: content.id },
					}"
				>
					Edit
				</ForgeButton>
				<GlobalAuthWrapper :restricted-to="['Admin']">
					<ForgeButton
						text
						@click.stop="dialog = true"
					>
						Archive
					</ForgeButton>
				</GlobalAuthWrapper>
				<v-dialog
					v-model="dialog"
					width="auto"
				>
					<v-card>
						<v-card-title>Archive Asset</v-card-title>
						<v-card-text>
							Are you sure you want to archive
							{{ content.title }}?
						</v-card-text>
						<v-card-actions>
							<ForgeButton
								color="primary"
								@click="dialog = false"
							>
								Cancel
							</ForgeButton>
							<ForgeButton
								color="error"
								@click="deleteAsset(content.id)"
							>
								Archive
							</ForgeButton>
						</v-card-actions>
					</v-card>
				</v-dialog>
			</div>
		</v-card>
	</div>
</template>

<script>
import { mapActions } from "vuex";

export default {
	name: "ContentLibraryIndexContentCard",
	props: {
		content: {
			type: Object,
			required: true,
		},
		isGrid: {
			type: Boolean,
			default: true,
		},
	},
	data() {
		return {
			dialog: false,
		};
	},
	computed: {
		contentCardClasses() {
			return this.isGrid
				? "index-content-card"
				: "index-content-card index-content-card__list";
		},
	},
	methods: {
		...mapActions("library", ["deleteContent"]),
		async deleteAsset(id) {
			await this.deleteContent(id);
			this.dialog = false;
		},
	},
};
</script>

<style
	lang="scss"
	scoped
>
.index-content {
	&-card {
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		align-items: center;
		width: 19rem;
		min-height: 20rem;
		margin: 0 1rem 1rem 0;
		padding-top: 1.5rem;
		background: var(--_core__container_background);
		&__list {
			flex-direction: row;
			padding-top: 0;
			width: 100%;
			min-height: 5rem;
			height: 5rem;
			& .index-content-card__image {
				height: 4rem;
			}
		}
		&__image {
			max-width: 17rem;
		}
		&__button-container {
			padding: 1rem;
		}
		& .v-card__title {
			font-size: 1rem;
			font-weight: bold;
			color: var(--core__color_primary);
		}
		& .v-card__subtitle {
			font-size: 0.7rem;
			color: var(--core__color_primary);
		}
	}
}
</style>
