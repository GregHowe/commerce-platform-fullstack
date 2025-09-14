<template>
	<v-app>
		<ValidationObserver ref="content">
			<TopNav />
			<v-main>
				<ForgeDrawer
					app
					left
					clipped
					permanent
					expand-on-hover
				>
					<DrawerMenuBase left-nav-type="library" />
				</ForgeDrawer>
				<ForgeProgressBar :active="isBusyAnything" />
				<transition
					name="quickfade"
					mode="in-out"
				>
					<Nuxt data-app />
				</transition>
			</v-main>
			<DrawerSettings />
		</ValidationObserver>
		<GlobalNotification />
		<GlobalFooter />
		<GlobalLoader />
	</v-app>
</template>
<script>
import { mapGetters, mapMutations } from "vuex";
export default {
	name: "LayoutDefault",
	computed: {
		...mapGetters("interface", ["isBusyAnything"]),
	},
	mounted() {
		// watch for any changes to ref "fields", which includes errors and input state.
		// pass the object to vuex when changed
		this.$watch(
			() => {
				return this.$refs.content.fields;
			},
			(val) => {
				this.setValidation(val);
			}
		);
	},
	methods: {
		...mapMutations("validation", ["setValidation"]),
	},
};
</script>
