<template>
	<v-card
		v-show="isQueued"
		max-width="200px"
	>
		<v-card-text>
			<div class="">
				<span :class="`${progressTextColor} font-weight-bold mr-3`"
					>Place in line: {{ placeInLine }} of
					{{ totalInQueue }}</span
				>
			</div>

			<div class="black--text text-caption">
				<v-progress-linear
					class="my-1"
					:color="progressColor"
					indeterminate
					striped
					height="6"
				/>

				<span v-show="timeEstimate && !publishFinishing"
					><v-icon small>mdi-clock-outline</v-icon>
					{{ minutesSeconds }}</span
				>
				<span v-show="timeEstimate && publishFinishing"
					><v-icon small>mdi-clock-outline</v-icon> Finishing...</span
				>
			</div>
		</v-card-text>
	</v-card>
</template>
<script>
import { mapState, mapGetters } from "vuex";

function millisToMinutesAndSeconds(millis) {
	var minutes = Math.floor(millis / 60000);
	var seconds = ((millis % 60000) / 1000).toFixed(0);
	return minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
}

export default {
	data() {
		return {
			timer: {},
			timeEstimate: "",
		};
	},
	computed: {
		...mapState({
			queue: (state) => state.buildQueue.queue,
			workingSite: (state) => state.site.workingSite,
		}),
		...mapGetters({
			placeInLine: "buildQueue/placeInLine",
			totalInQueue: "buildQueue/totalInQueue",
			isQueued: "buildQueue/isQueued",
			isPublishing: "buildQueue/isPublishing",
			siteBuild: "buildQueue/siteBuild",
		}),

		percentComplete() {
			return (this.placeInLine / this.totalInQueue) * 100;
		},
		publishText() {
			return this.isPublishing
				? "Your site is now being processed!"
				: "Your site is scheduled to be published!";
		},
		progressColor() {
			return this.isPublishing ? "green" : "blue";
		},
		progressTextColor() {
			return this.isPublishing ? "green--text" : "blue--text";
		},
		publishFinishing() {
			return this.timeEstimate < 5000;
		},
		siteBuild() {
			return this.queue.find(
				(q) =>
					q?.templateParameters?.CLOUDCMS_SITEID ===
					this.workingSite.id
			);
		},
		minutesSeconds() {
			return millisToMinutesAndSeconds(this.timeEstimate);
		},
	},
	mounted() {
		this.timer = setInterval(() => {
			this.timeEstimate =
				360000 * this.placeInLine -
				Math.abs(new Date(this.siteBuild?.createdDate) - new Date());
		}, 1000);
	},
	beforeDestroy() {
		clearInterval(this.timer);
	},
};
</script>
<style
	lang="scss"
	scoped
>
.description {
	font-size: 16px;
	line-height: 21px;
}
</style>
