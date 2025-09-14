<template>
	<v-app>
		<TopNav />
		<v-main>
			<transition
				name="quickfade"
				mode="in-out"
			>
				<Nuxt data-app />
			</transition>
		</v-main>
		<GlobalNotification />
		<GlobalFooter />
		<GlobalLoader />
	</v-app>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	name: "LayoutHeaderOnly",
	computed: {
		...mapGetters("interface", ["isBusyAnything"]),
	},
};
</script>
