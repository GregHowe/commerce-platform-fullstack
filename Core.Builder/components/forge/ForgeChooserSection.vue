<template>
	<v-dialog
		:value="true"
		fullscreen
		hide-overlay
		transition="dialog-bottom-transition"
	>
		<v-card light>
			<v-btn
				icon
				light
				absolute
				top
				right
				@click="$emit('close')"
			>
				<v-icon>mdi-close</v-icon>
			</v-btn>
			<v-card-title>
				Edit
				{{ activeSchema === "sections" ? "Section" : "Container" }}
			</v-card-title>
			<v-card-subtitle style="padding: 0 24px">
				Select
				{{
					activeSchema === "sections"
						? "a premade section"
						: "an empty container"
				}}
				to add to your page.
			</v-card-subtitle>
			<v-card-text style="padding: 0 24px">
				<a @click="toggleSchema">
					{{
						activeSchema === "sections"
							? "Add an empty container instead."
							: "Add a premade section instead."
					}}
				</a>
			</v-card-text>
			<v-container fluid>
				<v-row>
					<v-col
						cols="12"
						md="3"
					>
						<v-list>
							<v-list-item
								v-for="category in categories"
								:key="category.key"
								@click="showCategory(category.key)"
								>{{ category.label }}
							</v-list-item>
						</v-list>
					</v-col>
					<v-col cols="9">
						<v-row>
							<template v-if="!activeCategory">
								<p>No containers found.</p>
							</template>
							<template v-else>
								<v-col
									v-for="preset in activeCategory.presets"
									:key="preset.key"
									cols="4"
								>
									<v-card @click="choosePreset(preset.key)">
										<v-img
											src="blank.png"
											height="200"
										/>
										<v-card-title>
											{{ preset.label }}
										</v-card-title>
										<v-card-text>{{
											preset.description
										}}</v-card-text>
									</v-card>
								</v-col>
							</template>
						</v-row>
					</v-col>
				</v-row>
			</v-container>
		</v-card>
	</v-dialog>
</template>
<script>
import containers from "~/schemas/container.json";

import sections from "~/schemas/section.json";
import sectionsHero from "~/schemas/section/hero.json";
import sectionsForm from "~/schemas/section/form.json";
import sectionsContent from "~/schemas/section/content.json";
import sectionsNewsletter from "~/schemas/section/newsletter.json";
import sectionsDeveloper from "~/schemas/section/developer.json";
import sectionsAccordion from "~/schemas/section/accordion.json";
import sectionsPencilBanner from "~/schemas/section/pencilBanner.json";

export default {
	data() {
		return {
			activeSchema: "sections",
			activeCategoryKey: null,
		};
	},
	computed: {
		schema() {
			return this.activeSchema === "sections"
				? this.sectionsCombined
				: containers;
		},
		sectionsCombined() {
			return {
				...sections,
				categories: [
					sectionsHero,
					sectionsForm,
					sectionsContent,
					sectionsNewsletter,
					sectionsDeveloper,
					sectionsAccordion,
					sectionsPencilBanner,
				],
			};
		},
		categories() {
			return this.schema?.categories || [];
		},
		activeCategory() {
			if (!this.activeCategoryKey) {
				return null;
			}
			return this.categories.find(
				(c) => c.key === this.activeCategoryKey
			);
		},
	},
	mounted() {
		this.activeCategoryKey = this.categories[0].key;
	},
	methods: {
		choosePreset(key) {
			const preset = this.activeCategory.presets.find(
				(p) => p.key === key
			);
			this.$emit("choose", preset.blocks);
		},
		showCategory(key) {
			this.activeCategoryKey = key;
		},
		toggleSchema() {
			if (this.activeSchema !== "sections") {
				this.activeSchema = "sections";
			} else {
				this.activeSchema = "containers";
			}
		},
	},
};
</script>
<style
	lang="scss"
	scoped
>
.v-dialog > .v-card > .v-card__title {
	font-size: 2.25rem;
	font-weight: 800;
	margin: 0 0 0.5rem;
}
.v-dialog > .v-card > .v-card__subtitle {
	font-size: 1.2rem;
	margin: 0 0 0.2rem;
}
.v-dialog > .v-card > .container {
	margin-top: 2rem;
}
</style>
