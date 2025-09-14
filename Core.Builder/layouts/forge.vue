<template>
	<v-app
		:class="{
			'no-overflow': isSettingsMaximized,
		}"
	>
		<ValidationObserver ref="form">
			<v-main>
				<DrawerSettings />
				<ForgeDrawer
					app
					left
				>
					<DrawerMenuForge />
					<template #[`append`]>
						<v-list>
							<v-list-item>
								<v-list-item-content>
									<v-list-item-title class="font-weight-bold">
										<v-icon
											small
											color="black"
											style="top: -2px"
											class="mr-1"
										>
											mdi-pen
										</v-icon>
										Changing it up?
									</v-list-item-title>
									<div class="my-3 font-weight-medium">
										Learn how to use Site Styles to create
										custom looks.
									</div>
									<v-list-item-action class="ml-0">
										<NuxtLink
											class="userguidelink"
											to="#"
										>
											Visit User Guides
											<v-icon>mdi-chevron-right</v-icon>
										</NuxtLink>
									</v-list-item-action>
								</v-list-item-content>
							</v-list-item>
						</v-list>
					</template>
				</ForgeDrawer>

				<transition
					name="quickfade"
					mode="in-out"
				>
					<Nuxt data-app />
				</transition>
			</v-main>
		</ValidationObserver>
		<GlobalNotification />
		<ForgeSettingsMinimizer />
	</v-app>
</template>
<script>
import { mapGetters, mapMutations, mapState } from "vuex";
export default {
	name: "LayoutForge",
	validate({ params }) {
		return !!params.siteId && params.pageId;
	},
	computed: {
		...mapGetters("interface", ["isBusyAnything"]),
		...mapState("interface", ["isSettingsMaximized"]),
	},
	mounted() {
		// watch for any changes to ref "fields", which includes errors and input state.
		// pass the object to vuex when changed
		this.$watch(
			() => {
				return this.$refs.form.fields;
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
<style scoped>
.no-overflow {
	overflow: hidden;
	height: 100vh;
}
.userguidelink {
	text-decoration: none;
	font-weight: 500;
}
</style>
