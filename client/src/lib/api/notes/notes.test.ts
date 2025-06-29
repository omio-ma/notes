import type { Note } from "../../types";
import mockedAgent from "../../mockedAgent";
import { createNote, getAllNotes, updateNote } from "./notes";

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

  describe("createNote", () => {
    it("sends POST request to create a note", async () => {
      const requestPayload = {
        title: "New Note",
        content: "This is a new note"
      };

      const mockResponse = {
        data: undefined,
        headers: {
          location: "/notes/14"
        },
        status: 201,
        statusText: "Created",
        config: {}
      };

      mockedAgent.post.mockResolvedValueOnce(mockResponse);

      const result = await createNote(requestPayload);

      expect(mockedAgent.post).toHaveBeenCalledWith("/notes", requestPayload);
      expect(result).toEqual(mockResponse);
    });
  });

  it("sends PUT request to update note and returns updated note", async () => {
    const id = 5;
    const requestPayload = {
      title: "t104",
      content: "c1 edit 2"
    };
    const updatedNote: Note = {
      id,
      ...requestPayload,
      createdAt: new Date().toISOString()
    };

    mockedAgent.put.mockResolvedValueOnce({ data: updatedNote });

    const result = await updateNote(id, requestPayload);

    expect(mockedAgent.put).toHaveBeenCalledWith(
      `/notes/${id}`,
      requestPayload
    );
    expect(result).toEqual(updatedNote);
  });
});
