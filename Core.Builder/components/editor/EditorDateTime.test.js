import "@testing-library/jest-dom";
import EditorDateTime from "~/components/editor/EditorDateTime";
import Vuetify from "vuetify";
import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	return render(EditorDateTime, {
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			...propsData,
		},
	});
};

describe("EditorDateTime", () => {
	it("exists", () => {
		renderComponent({});
	});
});
