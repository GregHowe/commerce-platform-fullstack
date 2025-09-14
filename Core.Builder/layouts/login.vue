<template>
	<v-app>
		<v-main>
			<transition
				name="quickfade"
				mode="in-out"
			>
				<Nuxt data-app />
			</transition>
		</v-main>
		<GlobalNotification />
	</v-app>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	name: "LayoutLogin",
	computed: {
		...mapGetters("interface", ["isBusyAnything"]),
	},
};
</script>
