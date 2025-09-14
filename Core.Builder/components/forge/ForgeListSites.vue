<template>
	<v-list dense>
		<v-subheader>Your Sites</v-subheader>
		<template v-if="sites.length"> </template>
		<template v-else>
			<v-list-item>
				<v-list-item-content>
					<v-list-item-title> No sites yet. </v-list-item-title>
				</v-list-item-content>
			</v-list-item>
		</template>

		<v-list-item
			v-for="site in sites"
			:key="site.id"
			:to="`/forge/site/${site.id}`"
		>
			<v-list-item-content>
				<v-list-item-title>{{ site.title }}</v-list-item-title>
			</v-list-item-content>
			<v-list-item-action>
				<v-btn
					icon
					color="error"
					@click="
						(e) => {
							removeSite(site.id);
						}
					"
				>
					<v-icon>mdi-trash-can</v-icon>
				</v-btn>
			</v-list-item-action>
		</v-list-item>
		<v-divider />

		<v-list-item class="mt-3">
			<v-btn
				color="primary"
				@click="createSite"
			>
				<v-icon>mdi-plus</v-icon>
				Add New Site
			</v-btn>
		</v-list-item>
	</v-list>
</template>
<script>
import { mapActions } from "vuex";
export default {
	props: {
		sites: {
			type: Array,
			required: true,
		},
	},
	methods: {
		...mapActions("site", ["createSite", "removeSite"]),
	},
};
</script>
