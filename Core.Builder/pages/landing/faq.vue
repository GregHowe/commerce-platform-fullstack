<template>
	<v-container
		ref="faq"
		class="faq"
		:class="{
			mobile: mobileView || selfMobileView,
		}"
	>
		<div class="faqLabel">FAQ</div>
		<v-expansion-panels
			class="mb-6"
			:accordion="true"
			flat
			:multiple="false"
			:hover="true"
		>
			<v-expansion-panel
				v-for="(item, i) in faqlist"
				:key="i"
			>
				<v-expansion-panel-header
					hide-actions
					disable-icon-rotate
				>
					<template #default="{ open }">
						<div class="q">{{ faqlist[i].q }}</div>
						<v-icon>
							<template v-if="open">
								mdi-minus-circle-outline
							</template>
							<template v-else>
								mdi-plus-circle-outline
							</template>
						</v-icon>
					</template>
				</v-expansion-panel-header>
				<v-expansion-panel-content class="a">
					<div v-html="faqlist[i].a"></div>
				</v-expansion-panel-content>
			</v-expansion-panel>
		</v-expansion-panels>
	</v-container>
</template>

<script>
export default {
	name: "Faq",
	layout: "blank",
	auth: false,
	props: ["mobileView"],
	data: () => {
		let infoCentralLink = `<a href="#">[Agency Portal/Info Central]</a>`;
		return {
			selfMobileView: false,
			faqlist: [
				{
					q: `What is MySite powered by Core 2.0?`,
					a: `MySite powered by Core 2.0 is an easy-to-use website builder provided by Fusion 92 that will enable you to build your digital presence.`,
				},

				{
					q: `What will the impact be to my site?`,
					a: `<ul style='line-height: 30px;'><li>Your website will be migrated largely as is. We will maintain the integrity of the design for all custom sites. Templated websites will get a refreshed look and feel, with a modern design and new image options</li><li>We are also providing new features, including exclusive informational articles developed just for New York Life and lead forms integrated with Sales Central.</li></ul>`,
				},
				{
					q: `Will all my content be migrated?`,
					a: `All content currently visible on your website will be migrated to the new platform. Any custom content you've created that is currently approved by SMRU will be migrated as is. We are making some updates to library content, including a new article experience.`,
				},
				{
					q: `How will this impact my site analytics and/or current reporting?`,
					a: `Most existing metrics you receive from your website’s existing dashboards and reports will be available on Core 2.0 along with some new metrics. Your migrated website will be completely GA4 (Google Analytics 4) ready.`,
				},
				{
					q: `Will I be charged for this and when?`,
					a: `New York Life will cover the migration costs, including platform fees for the new platform throughout the migration period. After the migration period is complete, website owners will be responsible for the platform fees.`,
				},
				{
					q: `Can I opt to not migrate my site?`,
					a: `<ul style='line-height: 30px;'><li>All approved field websites must be hosted on MySite.</li><li>At the end of September 2023, the current Broadridge platform will no longer be approved or available. New York Life is migrating all field sites prior to the sunset date and covering the costs.</li></ul>`,
				},
				{
					q: `Who do I contact if I need help or have questions?`,
					a: `Questions should be directed to <a href="mailto:MarketingSupportInbox@newyorklife.com">MarketingSupportInbox@newyorklife.com</a>`,
				},
			],
		};
	},
	created() {
		this.updatePageWidth();
	},
	mounted() {
		window.addEventListener("resize", this.updatePageWidth);
	},
	destroyed() {
		window.removeEventListener("resize", this.updatePageWidth);
	},
	methods: {
		updatePageWidth() {
			if (this.mobileView) return;
			this.$nextTick(() => {
				if (window.innerWidth <= 767) {
					this.selfMobileView = true;
				} else {
					this.selfMobileView = false;
				}
			});
		},
	},
};
</script>

<style lang="scss">
.faq {
	width: 1440px;
	max-width: 100%;
	padding: 0;
	overflow: hidden;

	.faqLabel {
		font-weight: 400;
		font-size: 42px;
		line-height: 48px;
		text-align: center;
		color: #002840;
		margin: 50px 0;
	}

	.faqPanels {
		margin-bottom: 20px;
	}

	.v-expansion-panel-header i {
		justify-content: flex-end;
	}

	.v-expansion-panel {
		border-bottom: 2px solid #dae1e5;
	}

	.q {
		font-weight: 500;
		font-size: 28px;
		line-height: 32px;
		color: #002840;
		max-width: 95%;
	}

	.a {
		font-weight: 400;
		font-size: 16px;
		line-height: 24px;
		color: #243641;
		max-width: 95%;
	}

	.v-expansion-panel--active > .v-expansion-panel-header {
		min-height: unset;
	}

	&.mobile {
		position: relative;
		width: 100%;
		height: auto;

		.faqLabel {
			margin: 0;
			font-weight: 400;
			font-size: 34px;
			line-height: 40px;
		}

		.q {
			font-weight: 700;
			font-size: 16px;
			line-height: 20px;
			max-width: 90%;
		}

		.a {
			max-width: 90%;
		}
	}
}
</style>
