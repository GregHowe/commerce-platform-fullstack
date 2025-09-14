<template>
	<div class="drawer__body">
		<v-list-item two-line>
			<v-list-item-content>
				<v-list-item-title class="text-h5 mb-1">
					Theme Overrides
				</v-list-item-title>
				<v-list-item-subtitle>
					Select one of the following<br />
					options to override base styles.
				</v-list-item-subtitle>
			</v-list-item-content>
		</v-list-item>
		<v-list dense>
			<v-list-item>
				<v-btn
					:depressed="!selectedThemeBase"
					:disabled="!selectedThemeBase"
					@click="
						(e) => {
							applyTheme('base', null);
						}
					"
				>
					NYL Theme 1 - Defaults
				</v-btn>
			</v-list-item>
			<v-list-item
				v-for="theme in presetThemeBases"
				:key="theme.id"
			>
				<v-btn
					:depressed="selectedThemeBase === theme.id"
					:disabled="selectedThemeBase === theme.id"
					@click="
						(e) => {
							applyTheme('base', theme.id);
						}
					"
				>
					{{ theme.title }}
				</v-btn>
			</v-list-item>
		</v-list>
		<!-- <v-list-item two-line>
			<v-list-item-content>
				<v-list-item-title class="text-h5 mb-1">
					Theme Fonts
				</v-list-item-title>
				<v-list-item-subtitle>
					Change your site's fonts.
				</v-list-item-subtitle>
			</v-list-item-content>
		</v-list-item>
		<v-list dense>
			<v-list-item
				v-for="theme in presetThemeFonts"
				:key="theme.id"
			>
				<v-btn
					:depressed="selectedThemeFont === theme.id"
					:disabled="selectedThemeFont === theme.id"
					@click="
						(e) => {
							applyTheme('font', theme.id);
						}
					"
				>
					{{ theme.title }}
				</v-btn>
			</v-list-item>
		</v-list>
		<v-list-item two-line>
			<v-list-item-content>
				<v-list-item-title class="text-h5 mb-1">
					Theme Colors
				</v-list-item-title>
				<v-list-item-subtitle>
					Change your site's colors.
				</v-list-item-subtitle>
			</v-list-item-content>
		</v-list-item>
		<v-list dense>
			<v-list-item
				v-for="theme in presetThemeColors"
				:key="theme.id"
			>
				<v-btn
					:depressed="selectedThemeColor === theme.id"
					:disabled="selectedThemeColor === theme.id"
					@click="
						(e) => {
							applyTheme('color', theme.id);
						}
					"
				>
					{{ theme.title }}
				</v-btn>
			</v-list-item>
		</v-list> -->
	</div>
</template>
<script>
import themeSchema from "~/../Core.Library/src/schemas/themes.json";
import { mapMutations, mapState } from "vuex";
export default {
	name: "DrawerSettingsTheme",
	computed: {
		...mapState("site", ["themeList", "workingSite"]),
		presetThemeBases() {
			return themeSchema;
		},
		presetThemeColors() {
			try {
				return this.themeList.filter((theme) => {
					return theme.title.indexOf("olor") >= 0;
				});
			} catch (err) {
				return [];
			}
		},
		presetThemeFonts() {
			try {
				return this.themeList.filter((theme) => {
					return theme.title.indexOf("ont") >= 0;
				});
			} catch (err) {
				return [];
			}
		},
		selectedThemeColor() {
			return this.workingSite.themes?.color || null;
		},
		selectedThemeFont() {
			return this.workingSite.themes?.font || null;
		},
		selectedThemeBase() {
			return this.workingSite.themes?.base || null;
		},
	},
	methods: {
		...mapMutations("site", [
			"extendWorkingSite",
			"resetWorkingPageAndBlock",
			"setWorkingSiteSetting",
		]),
		applyTheme(themeType, themeId) {
			this.setWorkingSiteSetting({
				key: `themes.${themeType}`,
				value: themeId,
			});
		},
	},
};
</script>
