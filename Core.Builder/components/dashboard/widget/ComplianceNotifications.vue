<template>
	<DashboardModule
		label="Sites ready for Compliance Review"
		icon="mdi-shield-star"
	>
		<div>
			<v-data-table
				:headers="headers"
				:items="sites"
				:search="search"
				item-key="id"
				class="elevation-1"
			>
				<template #top>
					<v-text-field
						v-model="search"
						label="Search Website Titles"
						class="mx-4"
						prepend-inner-icon="mdi-magnify"
					></v-text-field>
				</template>
				<template #[`item.status`]="{ item }">
					<div>
						<v-chip
							small
							:color="
								item.status == 'LIVE'
									? 'success'
									: 'blue darken-1'
							"
							text-color="white"
						>
							{{ item.status }}
						</v-chip>
						<v-progress-circular
							v-show="item.status == 'PUBLISHING'"
							color="blue darken1"
							size="20"
							indeterminate
						/>
					</div>
				</template>
				<template #[`item.title`]="{ item }">
					<NuxtLink
						:to="{
							name: 'site-siteId',
							params: { siteId: item.id },
						}"
					>
						{{ item.title }}
					</NuxtLink>
				</template>
			</v-data-table>
		</div>
	</DashboardModule>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
	data() {
		return {
			search: "",
			headers: [
				{ text: "Status", value: "status" },
				{ text: "Title", value: "title" },
			],
		};
	},
	computed: {
		...mapState({
			siteList: (state) => state?.site?.siteList || [],
			queue: (state) => state.buildQueue.queue,
		}),
		sites() {
			return this.siteList.map((s) => ({
				status: this.queue.find(
					(q) =>
						q.templateParameters.CLOUDCMS_SITEID == s.id.toString()
				)
					? "PUBLISHING"
					: "LIVE",
				title: s.title,
				id: s.id,
			}));
		},
	},
	mounted() {
		this.GET_PIPELINE_STATUS();
	},
	methods: {
		...mapActions({
			GET_PIPELINE_STATUS: "buildQueue/GET_PIPELINE_STATUS",
		}),
		...mapActions("site", ["createSite", "removeSite"]),
		isPublishing(doc) {
			console.log(doc);
			return;
		},
	},
};
</script>
