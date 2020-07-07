import React, { Component } from "react";
import axios from "axios";

const url = "https://localhost:44359/api/file/upload";
const emptyItem = { name: "", description: "" };

class FileUpload extends Component {
  state = {
    item: { name: "", description: "" },
    selectedFile: null,
  };

  onChangeHandler = (e) => {
    this.setState({
      selectedFile: e.target.files,
    });
  };
  onClickHandler = async () => {
    const data = new FormData();

    for (var x = 0; x < this.state.selectedFile.length; x++) {
      data.append("files", this.state.selectedFile[x]);
    }
    for (var k in this.state.item) {
      data.append(k, this.state.item[k]);
    }

    const res = await axios.post(url, data, {}).then((res) => {
      if (res.status === 200) {
        const item = { ...emptyItem };
        const selectedFile = null; //{...this.state.item};
        this.setState({ item });
      }
    });
  };

  handleChange = (e) => {
    const item = { ...this.state.item };
    item[e.currentTarget.name] = e.currentTarget.value;
    this.setState({ item });
  };
  handleSubmit = (e) => {
    e.preventDefault();
  };

  render() {
    const { item } = this.state;

    return (
      <div className="container">
        <div className="row">
          <div className="col-md">
            <form onSubmit={this.handleSubmit}>
              <div className="form-group files">
                <label>Upload Your File </label>
                <input
                  type="file"
                  name="file"
                  multiple
                  onChange={this.onChangeHandler}
                />
              </div>
              <div className="form-group">
                <label for="name">Title</label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  value={this.state.item.name}
                  onChange={this.handleChange}
                ></input>
              </div>
              <div className="form-group">
                <label for="description">Description</label>
                <input
                  type="text"
                  id="description"
                  name="description"
                  value={this.state.item.description}
                  onChange={this.handleChange}
                ></input>
              </div>

              <button
                type="button"
                className="btn btn-success btn-block"
                onClick={this.onClickHandler}
              >
                Upload
              </button>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default FileUpload;
