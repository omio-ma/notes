import type { Note } from "../../types";
import mockedAgent from "../../mockedAgent";
import { getAllNotes } from "./notes";

describe("getAllNotes", () => {
  it("returns notes from the API", async () => {
    const fakeNotes: Note[] = [
      {
        id: 1,
        title: "Test Note",
        content: "This is a test note",
        createdAt: new Date().toISOString()
      }
    ];

    mockedAgent.get.mockResolvedValueOnce({ data: fakeNotes });

    const result = await getAllNotes();

    expect(result).toEqual(fakeNotes);
  });
});
