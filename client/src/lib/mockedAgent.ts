import agent from "./agent";

const mockedAgent = agent as jest.Mocked<typeof agent>;

jest.mock("./agent");

export default mockedAgent;
