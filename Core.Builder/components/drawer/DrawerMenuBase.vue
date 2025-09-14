<template>
	<div
		class="drawer__body"
		@mouseenter="over"
		@mouseleave="out"
	>
		<v-list
			dense
			nav
		>
			<v-list-item :style="overTitle">
				<v-list-item-content
					v-if="leftNavType === 'site'"
					class=""
				>
					<v-list-item-subtitle class=""> </v-list-item-subtitle>
					<v-list-item-title class="py-2"
						><h2>Manage your Site</h2>
					</v-list-item-title>
				</v-list-item-content>
			</v-list-item>
			<div
				v-for="link in leftLinks"
				:key="link.pathName"
			>
				<v-list-item
					v-if="link.subNav.length == 0"
					class="navItem"
					:to="{
						name: link.pathName,
						params: {
							siteId,
							pageId,
						},
					}"
					exact
					active-class="navItem-active"
				>
					<v-list-item-icon>
						<v-icon>{{ link.icon }}</v-icon>
					</v-list-item-icon>

					<v-list-item-content>
						<v-list-item-title>{{ link.title }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>

				<v-list-group
					v-else
					class="subnavItem"
				>
					<template #activator>
						<v-list-item>
							<v-list-item-icon>
								<v-icon>{{ link.icon }}</v-icon>
							</v-list-item-icon>
							<v-list-item-title>{{
								link.title
							}}</v-list-item-title>
						</v-list-item>
					</template>
					<div v-if="isOver">
						<v-list-item
							v-for="child in link.subNav"
							:key="child.title"
							:to="{
								path: `${siteId}/${child.path}`,
							}"
						>
							<v-list-item-content>
								<v-list-item-title class="ml-11">{{
									child.title
								}}</v-list-item-title>
							</v-list-item-content>
						</v-list-item>
					</div>
				</v-list-group>
			</div>
		</v-list>
	</div>
</template>

<script>
import { mapGetters, mapState } from "vuex";
export default {
	name: "SiteDrawer",
	props: {
		leftNavType: {
			type: String,
			default: "site",
		},
	},
	data() {
		return {
			isOver: false,
		};
	},
	computed: {
		...mapState("site", ["storedSite"]),
		...mapGetters("nav", ["leftDrawerLinks"]),
		siteId() {
			return this.storedSite ? this.storedSite.id : null;
		},
		pageId() {
			return this.storedSite?.pages.length
				? this.storedSite.pages[0].id
				: null;
		},
		overTitle() {
			return !this.isOver ? "opacity: 0" : "";
		},
		leftLinks() {
			return this.leftDrawerLinks(this.leftNavType);
		},
	},
	methods: {
		over() {
			this.isOver = true;
		},
		out() {
			this.isOver = false;
		},
	},
};
</script>
<style
	lang="scss"
	scoped
>
.v-list-item__subtitle h1 {
	color: black !important;
}

.v-application--is-ltr .v-list-item__action:first-child,
.v-application--is-ltr .v-list-item__icon:first-child {
	margin-right: 12px;
}

.v-icon {
	color: #343434 !important;
}

.navItem {
	margin-bottom: 10px;
	color: black !important;
	background: white !important;
}

.navItem-active {
	background: #343434 !important;
	color: white !important;
}

.navItem-active .v-icon {
	color: white !important;
}

.navItem:hover,
.subnavItem:hover {
	background: #343434 !important;
	color: white !important;
}

.subnavItem {
	margin-bottom: 10px;
	color: black !important;
	background: white !important;
	border-radius: 4px;
}

.subnavItem:hover .v-list-item__title {
	color: white !important;
}

.subnavItem:hover .v-icon {
	color: white !important;
}

.navItem:hover .v-icon {
	color: white !important;
}

.subnavItem .v-list-item {
	padding: 0;
}

.v-list-item {
	min-height: 30px;
}

.subnavItem a:last-child {
	padding-bottom: 15px !important;
}

.v-list--nav.v-list--dense .v-list-item:not(:only-child) {
	margin-bottom: 0px;
}
</style>
