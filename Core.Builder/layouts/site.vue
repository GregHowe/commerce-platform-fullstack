<template>
	<v-app>
		<TopNavSiteDetail />
		<v-main class="pl-14">
			<transition
				name="quickfade"
				mode="in-out"
			>
				<Nuxt data-app />
			</transition>
		</v-main>
		<ForgeDrawer
			left
			clipped
			permanent
			expand-on-hover
			absolute
			class="mt-16"
		>
			<DrawerMenuBase />
		</ForgeDrawer>
		<GlobalNotification />
		<ForgeSettingsMinimizer />
		<GlobalLoader />
	</v-app>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	name: "LayoutSite",
	validate({ params }) {
		return !!params.siteId;
	},
	computed: {
		...mapGetters("interface", ["isBusyAnything"]),
	},
};
</script>
