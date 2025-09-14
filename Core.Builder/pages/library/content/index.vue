<template>
	<div>
		<ContentLibraryHeader
			heading="Content Library"
			sub-heading="Lorem
		ipsum dolor sit amet, consectetur adipiscing elit. Est augue donec non
		varius etiam vel."
		>
			<ForgeButton
				text
				:to="{ name: 'library-content-new' }"
				class="primary mr-4"
				>Add New Content</ForgeButton
			>
		</ContentLibraryHeader>
		<div class="d-flex">
			<v-tabs v-model="tab">
				<v-tab
					v-for="item in tabItems"
					:key="item.id"
				>
					{{ item.label }}
				</v-tab>
			</v-tabs>
			<v-col
				cols="auto"
				align-self-center
			>
				<div class="toggle_buttons">
					<ForgeButton
						text
						:class="{ primary: isGridLayout }"
						@click="isGridLayout = true"
					>
						<v-icon>mdi-view-grid-outline</v-icon>
						Grid
					</ForgeButton>
					<ForgeButton
						text
						:class="{ primary: !isGridLayout }"
						@click="isGridLayout = false"
					>
						<v-icon>mdi-list-box-outline</v-icon>
						List
					</ForgeButton>
				</div>
			</v-col>
			<v-col
				cols="auto"
				align-self-center
			>
				<v-select
					v-model="selectedSort"
					dense
					solo
					flat
					outlined
					prefix="Sort By:"
					style="max-width: 250px"
					:items="['Date Added', 'Type']"
				/>
			</v-col>
		</div>
		<v-tabs-items v-model="tab">
			<v-tab-item
				v-for="item in tabItems"
				:key="item.id"
			>
				<v-card flat>
					<v-row v-if="item.id === 1">
						<v-col
							cols="12"
							md="3"
							class="fill-height"
						>
							<v-list>
								<v-list-item>
									<v-list-item-title
										class="text-uppercase font-weight-bold"
										>Your Filters
									</v-list-item-title>
									<v-list-item-action>
										<ForgeButton
											text
											icon
											@click="deleteAllFilters"
										>
											<v-icon color="primary"
												>mdi-delete-outline</v-icon
											>
										</ForgeButton>
									</v-list-item-action>
								</v-list-item>

								<v-list-item class="flex-wrap">
									<ForgeChip
										v-for="filter in filters"
										:key="`${filter.value}${filter.id}`"
										:name="filter.label"
										color="primary"
										class="ma-2"
										close
										@click:close="deleteFilter(filter)"
									/>
								</v-list-item>
								<v-list-group :value="false">
									<template #activator>
										<v-list-item-title
											class="text-uppercase font-weight-bold"
											>Category
										</v-list-item-title>
									</template>

									<v-list-item-content>
										<v-list-item class="flex-wrap">
											<ForgeChip
												v-for="categoryItem in filterCategories"
												:key="categoryItem.id"
												pill
												:name="categoryItem.label"
												color="primary"
												class="ma-2"
												:disabled="
													categoryItem.selected
												"
												@click="addFilter(categoryItem)"
											/>
										</v-list-item>
									</v-list-item-content>
								</v-list-group>
							</v-list>
						</v-col>
						<v-col
							cols="12"
							md="8"
							:class="`${isGridLayout ? 'd-flex flex-wrap' : ''}`"
						>
							<div
								v-for="contentItem in content"
								:key="contentItem.id"
							>
								<ContentLibraryIndexContentCard
									:content="contentItem"
									:is-grid="isGridLayout"
								/>
							</div>
						</v-col>
					</v-row>
				</v-card>
			</v-tab-item>
		</v-tabs-items>
	</div>
</template>
<script>
import { mapState, mapActions } from "vuex";
export default {
	layout: "default",
	data() {
		return {
			tab: null,
			isGridLayout: true,
			selectedSort: "Date Added",
			tabItems: [
				{ id: 1, label: "All Media" },
				{ id: 2, label: "Recently Added By Me" },
			],
		};
	},
	computed: {
		...mapState("library", ["content", "filters", "categories"]),
		filterCategories() {
			if (this.categories) {
				return this.categories.map((cat) => {
					return {
						value: cat.id,
						label: cat.title,
						name: cat.title,
					};
				});
			}
			return [];
		},
	},
	methods: {
		...mapActions("library", [
			"deleteAllFilters",
			"deleteFilter",
			"addFilter",
		]),
	},
};
</script>
